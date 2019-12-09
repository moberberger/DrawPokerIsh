using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CardSpriteRequest
{
    public int CardId;

    public Sprite Sprite { get; set; }

    public CardSpriteRequest( int _cardId ) => CardId = _cardId;
}
