using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// ESuit describes the 4 suits found in a "Standard" deck of cards. It is
/// important to retain the specific int values associated with each card suit
/// in order to effectively map card values to suit names. This is most
/// important when mapping to card image files on disk.
/// </summary>
public enum ESuit
{
    _invalid = -1,
    Heart = 0,
    Diamond = 1,
    Club = 2,
    Spade = 3,
    Joker = 4,
}
