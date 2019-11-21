using Protobuf.Cards;
using Protobuf.DrawPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using Morpheus;

namespace DrawPoker
{
    public class FiveCardNoWildsLogic : PokerEvaluationLogic
    {
        public override int ValidWildCardCount => 0;
        public override int DeckSize => 52;

        /// <summary>
        /// Go over the cards to determine which basic poker hands are present for a 5-card hand
        /// with no wild cards using a standard 52-card deck (i.e. no duplicate cards)
        /// </summary>
        /// <param name="cards">A list of cards to transform into basic Poker data</param>
        public override Evaluation AnalyzeCards( IList<PlayingCard> cards )
        {
            //Log.Trace();
            //Log.Info( $"Analysing cards: {cards.JoinAsString( " " )}" );

            VerifyFiveUniqueCards( cards );

            InternalOrderedRanksAndFlush( cards );

            InternalAnalyzeDistinctRankData();

            return ApplyLogicToEvaluation( Evaluation );
        }

        public static Evaluation ApplyLogicToEvaluation( Evaluation eval )
        {
            // The only remaining operations are O(1) checks based on the number of distinct
            // ranks. Remember that wild cards aren't supported by this algorithm.
            switch (eval.DistinctRankCount)
            {
            case 2: // must be either 1+4 or 2+3
                eval.FourOfAKind = (eval.DistinctRankCountProduct == 4);
                eval.FullHouse = (eval.DistinctRankCountProduct == 6);
                break;
            case 3: // must be either 3+1+1 or 2+2+1
                eval.ThreeOfAKind = (eval.DistinctRankCountProduct == 3);
                eval.TwoPair = (eval.DistinctRankCountProduct == 4);
                break;
            case 4: // Must be 2+1+1+1
                eval.Pair = true;
                break;
            case 5: // All cards are different- May be a straight (Flush already determined)
                eval.Straight = (eval.LastCard - eval.FirstCard == 4);
                eval.Straight |= (eval.FirstCard == ERank._2 &&
                                        eval.OrderedRanks[3] == ERank._5 &&
                                        eval.LastCard == ERank.Ace);
                break;
            default:
                throw new ArgumentException( $"The cards you passed in resulted in {eval.DistinctRankCount} distinct ranks, which is invalid." );
            }

            return eval;
        }
    }
}
