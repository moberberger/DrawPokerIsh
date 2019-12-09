/// <summary>
/// The enumeration for translating an integral "Rank" value into a card "Rank".
/// It is important to retain the specific int values associated with each card
/// rank in order to effectively map card values to suit names. This is most
/// important when mapping to card image files on disk.
/// </summary>
public enum ERank
{
    _invalid = -1,

    _2 = 0,
    _3,
    _4,
    _5,
    _6,
    _7,
    _8,
    _9,
    _10,
    _Jack,
    _Queen,
    _King,
    _Ace,
    _Joker
}
