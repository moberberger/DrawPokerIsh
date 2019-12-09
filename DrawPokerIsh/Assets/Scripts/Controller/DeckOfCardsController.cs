
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

        if (cardId < 0 || cardId > 51)
            _req.Sprite = MissingCardSprite;
        else
            _req.Sprite = CardsInDeckInOrder[cardId];
    }
}

