
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG;
using DG.Tweening;
using DG.Tweening.Core;
using Morpheus;
using System.Collections;
using Protobuf.Cards;

public class DeckOfCardsController : MonoBehaviour
{
    public Sprite[] CardsInDeckInOrder;
    public Sprite MissingCardSprite;


    [AEventHandler]
    public void ProvideSprite( CardSpriteRequest _req )
    {
        var cardId = _req.CardId;
        _req.Sprite = GetSprite( cardId );
    }

    public Sprite GetSprite( int cardId )
    {
        if (cardId < 0 || cardId > 51)
            return MissingCardSprite;
        else
            return CardsInDeckInOrder[cardId];
    }
}

