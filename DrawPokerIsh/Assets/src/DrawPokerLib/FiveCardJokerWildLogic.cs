using Protobuf.Cards;
using Protobuf.DrawPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using Morpheus;


namespace DrawPoker
{
    public class FiveCardJokerWildLogic : PokerEvaluationLogic
    {
        public override int ValidWildCardCount => 1;
        public override int DeckSize => 53;

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

            var cardsWithoutWild = cards.Where( _c => _c.Rank != ERank._2 ).ToList();
            if (cardsWithoutWild.Count == 5) // no joker- analyze as a normal 5-card no-wild hand.
                return Evaluation = new FiveCardNoWildsLogic().AnalyzeCards( cards );

            // There's a single wildcard, so process as such
            Evaluation.NumberOfWildCards = 1;
            InternalOrderedRanksAndFlush( cardsWithoutWild );
            InternalAnalyzeDistinctRankData();
            return ApplyLogicToEvaluation( Evaluation );
        }

        public static Evaluation ApplyLogicToEvaluation( Evaluation eval )
        {
            // The only remaining operations are O(1) checks based on the number of distinct
            // ranks. All evaluations are done knowing that there's a joker that completes
            // the 4-card hand.
            switch (eval.DistinctRankCount)
            {
            case 1: // 4 cards and 1 wild means 5 of a kind
                eval.FiveOfAKind = true;
                break;
            case 2: // must be either 1+3 or 2+2
                eval.FourOfAKind = (eval.DistinctRankCountProduct == 3);
                eval.FullHouse = (eval.DistinctRankCountProduct == 4);
                break;
            case 3: // 2+1+1 could be two-pair, but three-of-a-kind is higher ranked.
                eval.ThreeOfAKind = true;
                break;
            case 4: // All cards are different- May be a straight (Flush already determined)
                eval.Straight = (eval.LastCard - eval.FirstCard <= 4);
                eval.Straight |= eval.OrderedRanks[2] <= ERank._5 &&
                                       eval.LastCard == ERank.Ace;
                if (!eval.Straight) // Not a straight, so it must be a simple pair.
                {
                    eval.Pair = true;
                    eval.HighestDuplicateRank = eval.LastCard;
                }
                break;
            default:
                throw new ArgumentException( $"The cards you passed in resulted in {eval.DistinctRankCount} distinct ranks, which is invalid." );
            }
            return eval;
        }
    }
}

