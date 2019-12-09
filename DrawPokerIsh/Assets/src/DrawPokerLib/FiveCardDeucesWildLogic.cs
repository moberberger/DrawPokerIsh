using Protobuf.Cards;
using Protobuf.DrawPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using Morpheus;

namespace DrawPoker
{
    public class FiveCardDeucesWildLogic : PokerEvaluationLogic
    {
        public override int ValidWildCardCount => 4;
        public override int DeckSize => 52;

        /// <summary>
        /// Go over the cards to determine which basic poker hands are present for a 5-card hand
        /// using a standard 52-card deck (i.e. no duplicate cards) with the possibility of a
        /// Joker
        /// </summary>
        /// <param name="cards">A list of cards to transform into basic Poker data</param>
        public override Evaluation AnalyzeCards( IList<PlayingCard> cards )
        {
            //Log.Trace();
            //Log.Info( $"Analysing cards: {cards.JoinAsString( " " )}" );

            VerifyFiveUniqueCards( cards );

            // Figure out how many wild cards there are
            var cardsWithoutWild = cards.Where( _c => _c.Rank != ERank._2 ).ToList();
            Evaluation.NumberOfWildCards = 5 - cardsWithoutWild.Count;

            // Decide what to do based on wild card count
            switch (Evaluation.NumberOfWildCards)
            {
            case 0: // Standard poker logic
                Evaluation = new FiveCardNoWildsLogic().AnalyzeCards( cards );
                break;
            case 1: // The Joker Wild logic will handle any single wild card
                Evaluation = new FiveCardJokerWildLogic().AnalyzeCards( cards );
                break;
            case 4: // Deuces Wild- 4 Dueces + kicker
                Evaluation.DistinctRankCount = 2;
                Evaluation.OrderedRanks.Add( cardsWithoutWild[0].Rank );
                Evaluation.UnorderedCards.AddRange( cards );
                Evaluation.FourOfAKind = true;
                Evaluation.HighestDuplicateRank = ERank._2;
                break;

            default: // There must be either 2 or 3 Wild Cards
                InternalOrderedRanksAndFlush( cardsWithoutWild );
                InternalAnalyzeDistinctRankData();

                if (Evaluation.DistinctRankCount == 1)
                {
                    // If all non-wild cards are the same rank, then we must have a...
                    Evaluation.FiveOfAKind = true;
                }
                else if (Evaluation.DistinctRankCount != Evaluation.OrderedRanks.Count)
                {
                    // If there are duplicates of rank (ie Any Pair), The FourOfAKind is the
                    // optimal hand. While this could only be true if there were 2 wildcards,
                    // testing in the case of 3 wild cards is harmless as the test will always
                    // fail due to the "Five of a Kind" test above.
                    Evaluation.FourOfAKind = true;
                }
                else if (Evaluation.NumberOfWildCards == 3) // 2 different extras
                {
                    // AT THIS POINT... With three wild cards, the worst possible hand is a
                    // four-of-a-kind. This only happens IFF we don't have a
                    // straight(royal)-flush. In other words, with three wilds, we can only have
                    // a straight(royal)-flush or a four-kind
                    if (Evaluation.Flush && Evaluation.IsPossibleStraight()) // its both
                    {
                        Evaluation.Straight = true;
                    }
                    else
                    {
                        Evaluation.FourOfAKind = true;
                        Evaluation.Flush = false; // may have been set by Internal...
                    }
                }
                else // 2 Wildcards, 3 DIFFERENT extra cards
                {
                    // The three extra cards are different. So, we have AT LEAST a 3ofAKind.
                    // However, either a Straight or a Flush would be better, so check for those
                    // first.
                    Evaluation.Straight = Evaluation.IsPossibleStraight();
                    Evaluation.ThreeOfAKind = !Evaluation.Straight && !Evaluation.Flush;
                }
                break;
            }

            return Evaluation;
        }

    }
}
