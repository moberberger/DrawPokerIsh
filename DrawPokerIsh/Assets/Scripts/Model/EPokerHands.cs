using System;
using System.Collections.Generic;
using System.Text;

namespace Igt.HyperPoker
{
    public enum EPokerHands
    {
        Invalid = -1,
        HighCard = 0,
        Pair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }
}
