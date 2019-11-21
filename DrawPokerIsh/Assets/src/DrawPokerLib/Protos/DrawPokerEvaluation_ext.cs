using Protobuf.Cards;
using System.Linq;

namespace Protobuf.DrawPoker
{
    public partial class Evaluation
    {
        public Protobuf.Cards.ERank FirstCard => OrderedRanks[0];
        public Protobuf.Cards.ERank MiddleCard => OrderedRanks[OrderedRanks.Count / 2];
        public Protobuf.Cards.ERank LastCard => OrderedRanks[OrderedRanks.Count - 1];

        /// <summary>
        /// Test the Evaluation to see if the cards that are in the OrderedRanks array COULD be
        /// made into a straight assuming that there are enough wild cards to fill out the hand
        /// to 5 cards. THIS TEST ASSUMES THAT THE OrderedRanks HAVE NO DUPLICATES! In other
        /// words, make sure there are no pairs, etc, before calling this routine
        /// </summary>
        /// <returns></returns>
        public bool IsPossibleStraight()
        {
            if (OrderedRanks == null) return false;

            if ((LastCard - FirstCard) <= 4) // easy straight
                return true;

            if (LastCard == ERank.Ace) // Ace -> 5 straight
            {
                // Test the second-to-last card (the last card is an ACE) to see if its .le. _5.
                int idx = OrderedRanks.Count - 2;
                // Assume 0 or 1 cards is always a possible straight
                return (idx < 0) ? true : (OrderedRanks[idx] <= ERank._5);
            }

            return false;
        }

    }
}
