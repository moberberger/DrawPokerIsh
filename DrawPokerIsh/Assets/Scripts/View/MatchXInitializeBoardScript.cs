using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Morpheus;
using TMPro;
using UnityEngine.EventSystems;
using Protobuf.Cards;

[ExecuteInEditMode]
public class MatchXInitializeBoardScript : MonoBehaviour
{
    public DeckOfCardsController CardImages;
    public string[] RowsOfCardsSeparatedBySpaces = new string[5];

    void Update()
    {
        if (!Application.isPlaying)
        {
            GetComponentsInChildren<MatchXInitializeCardScript>().ForEach( ( cb, idx ) =>
            {
                var r = cb.Row = idx / 5;
                var c = cb.Column = idx % 5;
                cb.CardId = GetCardId( r, c );
                cb.Sprite = CardImages.GetSprite( cb.CardId );
            } );
        }
    }

    public int GetCardId( int r, int c )
    {
        var cards = RowsOfCardsSeparatedBySpaces[r];
        var listCards = cards.Split( ' ' ).Select( s => PlayingCard.IdFrom2String( s ) );
        return listCards.ElementAt( c );
    }
}

