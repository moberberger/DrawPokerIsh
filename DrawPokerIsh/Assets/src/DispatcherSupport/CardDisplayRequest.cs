using UnityEngine;


public class CardDisplayRequest
{
    public int CardIndex = -1;
    public int HandIndex = -1;
    public int CardId = -1;

    public CardDisplayRequest( int _cardIdx, int _handIdx, int _cardId )
    {
        CardIndex = _cardIdx;
        HandIndex = _handIdx;
        CardId = _cardId;
    }

    public bool IsMatch( int _handIndex, int _cardIndex )
    {
        if (HandIndex != -1 && _handIndex != -1 && HandIndex != _handIndex)
            return false;
        if (CardIndex != -1 && _cardIndex != -1 && CardIndex != _cardIndex)
            return false;
        return true;
    }
}