using Protobuf.DrawPoker;
using Protobuf.Cards;

namespace DrawPoker
{
    public static class BasicPaylineEvaluators
    {
        /// <summary>
        /// Helper to make sure checking if a rank is between two other ranks (inclusive) is
        /// more readable
        /// </summary>
        /// <param name="rank">The rank to check</param>
        /// <param name="lowRank">The lower rank to check against (inclusive)</param>
        /// <param name="highRank">The upper rank to check against (inclusive)</param>
        /// <returns>TRUE if the rank is between the two other ranks, inclusive</returns>
        public static bool InRange( this ERank rank, ERank lowRank, ERank highRank )
        {
            return rank >= lowRank && rank <= highRank;
        }

        /// <summary>
        /// Helper for 4-of-a-Kind evals, because there are MANY paytables that pay for
        /// "special" versions of the 4-of-a-Kind
        /// </summary>
        /// <param name="eval">The evaluation to use</param>
        /// <returns>(Rank of the 4 duplicate ranks, Rank of 5th card)</returns>
        public static void Get4CardAndOther( Evaluation eval, out ERank fourOfAKindRank, out ERank kicker )
        {
            if (eval.FourOfAKind)
            {
                fourOfAKindRank = eval.MiddleCard;
                kicker = (fourOfAKindRank == eval.FirstCard) ? eval.LastCard : eval.FirstCard;
            }
            else
            {
                fourOfAKindRank = ERank.Unknown;
                kicker = ERank.Unknown;
            }
        }


        [APaylineEvaluator( "Jacks Or Better" )]
        public static bool IsJacksOrBetter( Evaluation eval )
        {
            return (eval.Pair && eval.HighestDuplicateRank >= ERank.Jack);
        }

        [APaylineEvaluator( "Kings Or Better" )]
        public static bool IsKingsOrBetter( Evaluation eval )
        {
            return (!eval.Flush &&
                     eval.Pair &&
                     eval.HighestDuplicateRank >= ERank.King);
        }

        [APaylineEvaluator( "Two Pair" )]
        public static bool IsTwoPair( Evaluation eval )
        {
            return eval.TwoPair;
        }

        [APaylineEvaluator( "Three Of A Kind" )]
        public static bool IsThreeOfAKind( Evaluation eval )
        {
            return eval.ThreeOfAKind;
        }

        [APaylineEvaluator( "Five Of A Kind" )]
        public static bool IsFiveOfAKind( Evaluation eval )
        {
            return eval.FiveOfAKind;
        }

        [APaylineEvaluator( "Straight" )]
        public static bool IsStraight( Evaluation eval )
        {
            return eval.Straight && !eval.Flush;
        }

        [APaylineEvaluator( "Flush" )]
        public static bool IsFlush( Evaluation eval )
        {
            return eval.Flush && !eval.Straight;
        }

        [APaylineEvaluator( "Full House" )]
        public static bool IsFullHouse( Evaluation eval )
        {
            return eval.FullHouse;
        }

        [APaylineEvaluator( "Four Of A Kind" )]
        public static bool IsFourOfAKind( Evaluation eval )
        {
            return eval.FourOfAKind;
        }

        [APaylineEvaluator( "Straight Flush" )]
        public static bool IsStraightFlush( Evaluation eval )
        {
            return eval.Straight && eval.Flush && eval.FirstCard < ERank._10;
        }

        [APaylineEvaluator( "Royal Flush" )]
        public static bool IsRoyalFlush( Evaluation eval )
        {
            return eval.Straight && eval.Flush && eval.FirstCard == ERank._10;
        }

        [APaylineEvaluator( "Four Aces" )]
        public static bool IsFourAces( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard == ERank.Ace;
        }

        [APaylineEvaluator( "Four Deuces" )]
        public static bool IsFourDeuces( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard == ERank._2;
        }

        [APaylineEvaluator( "Four 2-K" )]
        public static bool IsFour2_K( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank._2, ERank.King );
        }

        [APaylineEvaluator( "Four J-K" )]
        public static bool IsFourJ_K( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank.Jack, ERank.King );
        }

        [APaylineEvaluator( "Four 2-4" )]
        public static bool IsFour2_4( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank._2, ERank._4 );
        }

        [APaylineEvaluator( "Four 5-10" )]
        public static bool IsFour5_10( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank._5, ERank._10 );
        }

        [APaylineEvaluator( "Four 5-K" )]
        public static bool IsFour5_K( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank._5, ERank.King );
        }

        [APaylineEvaluator( "Four Aces + 2-4" )]
        public static bool IsFourAces__2_4( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard == ERank.Ace &&
                   other.InRange( ERank._2, ERank._4 );
        }

        [APaylineEvaluator( "Four Aces + 5-10" )]
        public static bool IsFourAces__5_10( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard == ERank.Ace &&
                   other.InRange( ERank._5, ERank._10 );
        }

        [APaylineEvaluator( "Four Aces + 5-K" )]
        public static bool IsFourAces__5_k( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard == ERank.Ace &&
                   other.InRange( ERank._5, ERank.King );
        }

        [APaylineEvaluator( "Four Aces + J-K" )]
        public static bool IsFourAces__J_k( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard == ERank.Ace &&
                   other.InRange( ERank.Jack, ERank.King );
        }

        [APaylineEvaluator( "Four 2-4 + A-4" )]
        public static bool IsFour2_4__A_4( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank._2, ERank._4 ) &&
                   (other.InRange( ERank._2, ERank._4 ) ||
                    other == ERank.Ace);
        }

        [APaylineEvaluator( "Four 2-4 + 5-K" )]
        public static bool IsFour2_4__5_k( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank._2, ERank._4 ) &&
                   other.InRange( ERank._5, ERank.King );
        }

        [APaylineEvaluator( "Four J-K + J-A" )]
        public static bool IsFourJ_K__J_A( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank.Jack, ERank.King ) &&
                   (other.InRange( ERank.Jack, ERank.Ace ));
        }

        [APaylineEvaluator( "Four J-K + 2-10" )]
        public static bool IsFourJ_K__2_10( Evaluation eval )
        {
            Get4CardAndOther( eval, out var fourCard, out var other );
            return fourCard.InRange( ERank.Jack, ERank.King ) &&
                   (other.InRange( ERank._2, ERank._10 ));
        }

        [APaylineEvaluator( "Natural Royal Flush" )]
        public static bool IsNaturalRoyal( Evaluation eval )
        {
            return eval.Straight &&
                   eval.Flush &&
                   eval.FirstCard == ERank._10 &&
                   eval.NumberOfWildCards == 0;
        }

        [APaylineEvaluator( "Wild Royal Flush" )]
        public static bool IsWildRoyal( Evaluation eval )
        {
            return eval.Straight &&
                   eval.Flush &&
                   eval.FirstCard == ERank._10 &&
                   eval.NumberOfWildCards > 0;
        }
    }
}
