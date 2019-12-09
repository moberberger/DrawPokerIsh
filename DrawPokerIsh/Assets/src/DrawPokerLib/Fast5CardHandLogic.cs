using Protobuf.Cards;
using Protobuf.DrawPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DrawPoker
{
    class Fast5CardHandLogic : PokerEvaluationLogic
    {
        public override int ValidWildCardCount => 0;

        public override int DeckSize => 52;

        enum Possibility
        {
            Nothing = 0,
            Pair,
            ThreeOfAKind,
            FourOfAKind,
            Straight,
            FullHouse,
        }
        /// <summary>
        /// Go over the cards to determine which basic poker hands are present for a 5-card hand
        /// with no wild cards using a standard 52-card deck (i.e. no duplicate cards)
        /// </summary>
        /// <param name="cards">A list of cards to transform into basic Poker data</param>
        public override Evaluation AnalyzeCards( IList<PlayingCard> cards )
        {
            Evaluation.DistinctRankCount = 0; // signal failure

            // Any three of the 5 cards can be used to determine if this CANNOT be a win, so
            // perform that much faster check and prune the statespace here if possible The
            // entire purpose of this routine is PERFORMANCE. It takes advantage of the fact
            // that the first three cards (really any three cards) can be used to eliminate a
            // huge subset of the searchable statespace consisting of all permutations of five
            // cards.
            var possibility = Possibility.Nothing;

            var c1 = cards[0];
            var c2 = cards[1];
            var c3 = cards[2];
            var c4 = cards[3];
            var c5 = cards[4];

            // This means that the "Flush" check is 100% correct after this method is run.
            if (c1.Suit == c2.Suit &&
                c1.Suit == c3.Suit &&
                c1.Suit == c4.Suit &&
                c1.Suit == c5.Suit)
            {
                Evaluation.Flush = true;
            }

            // Dereference the ranks
            var r1 = (int) c1.Rank;
            var r2 = (int) c2.Rank;
            var r3 = (int) c3.Rank;

            // Sort the ranks. JIT should translate Interlocked.Exchange into a single ASM
            // instruction
            if (r2 < r1) r1 = Interlocked.Exchange( ref r2, r1 );
            if (r3 < r1) r1 = Interlocked.Exchange( ref r3, r1 );
            if (r3 < r2) r2 = Interlocked.Exchange( ref r3, r2 );

            // Find distances between ranks
            var d1 = r2 - r1;
            var d2 = r3 - r1;
            var d3 = r3 - r2;

            // At this point, a FLUSH means there can't be a pair, but could be a straignt
            if (Evaluation.Flush)
            {
                if (d1 < 5 && d2 < 5 && d3 < 5) // maybe a straight?
                {
                    // All deltas are within "straight" distance, so a straight is possible
                    possibility = Possibility.Straight;
                    Evaluation.DistinctRankCount = 5;
                }
            }
            else // not a flush, so maybe pairs, maybe straight
            {
                if (d1 == 0) // at least a pair, definitely not a straight
                {
                    if (d2 == 0) // 1 == 2 AND 1 == 3 implies...
                    {
                        possibility = Possibility.ThreeOfAKind;
                    }
                    else
                    {
                        possibility = Possibility.Pair;
                    }
                }
                else if (d2 == 0 || d3 == 0) // a pair, not a straight
                {
                    possibility = Possibility.Pair;
                }
                else if (d1 < 5 && d2 < 5 && d3 < 5) // maybe a straight?
                {
                    // All deltas are within "straight" distance, so a straight is possible
                    possibility = Possibility.Straight;
                }
            }
            // The 3 cards say that there CANT be a win, or the win is a FLUSH, in which case
            // the Evaluation object is already set up correctly, so return it.
            if (possibility == Possibility.Nothing)
                return Evaluation; // If FLUSH is set, then its still good


            // Introduce 4th and 5th card information
            var r4 = (int) c4.Rank;
            var r5 = (int) c5.Rank;
            if (r5 < r4) r4 = Interlocked.Exchange( ref r5, r4 );
            var d45 = r5 - r4;

            switch (possibility)
            {
            case Possibility.Pair:
                // 4 & 5 are pair- Check if same pair as in the three cards
                if (d45 == 0 && (r4 == r1 || r4 == r2))
                {
                    Evaluation.FourOfAKind = true;
                    return Evaluation;
                }
                break;
            case Possibility.ThreeOfAKind:
                break;
            case Possibility.FourOfAKind:
                break;
            case Possibility.Straight:
                break;
            case Possibility.FullHouse:
                break;
            default:
                break;
            }




            // VerifyFiveUniqueCards( cards );

            // InternalOrderedRanksAndFlush( cards );

            // InternalAnalyzeDistinctRankData();

            //<code>// The only remaining operations are O(1) checks based on the number of distinct
            //// ranks. Remember that wild cards aren't supported by this algorithm.
            //switch (Evaluation.DistinctRankCount)
            //{
            //case 2: // must be either 1+4 or 2+3
            //    Evaluation.FourOfAKind = (Evaluation.DistinctRankCountProduct == 4);
            //    Evaluation.FullHouse = (Evaluation.DistinctRankCountProduct == 6);
            //    break;
            //case 3: // must be either 3+1+1 or 2+2+1
            //    Evaluation.ThreeOfAKind = (Evaluation.DistinctRankCountProduct == 3);
            //    Evaluation.TwoPair = (Evaluation.DistinctRankCountProduct == 4);
            //    break;
            //case 4: // Must be 2+1+1+1
            //    Evaluation.Pair = true;
            //    break;
            //case 5: // All cards are different- May be a straight (Flush already determined)
            //    Evaluation.Straight = (Evaluation.LastCard - Evaluation.FirstCard == 4);
            //    Evaluation.Straight |= (Evaluation.FirstCard == ERank._2 &&
            //                            Evaluation.OrderedRanks[3] == ERank._5 &&
            //                            Evaluation.LastCard == ERank.Ace);
            //    break;
            //default:
            //    throw new ArgumentException( $"The cards you passed in resulted in {Evaluation.DistinctRankCount} distinct ranks, which is invalid." );
            //}

            return Evaluation;
        }

    }
}
