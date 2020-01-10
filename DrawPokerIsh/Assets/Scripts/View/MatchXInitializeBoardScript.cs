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

    void Start()
    {
        SetChildren();
    }

    void Update()
    {
        if (!Application.isPlaying)
            SetChildren();
    }

    private void SetChildren() => GetComponentsInChildren<MatchXInitializeCardScript>().ForEach( ( _cinit, _idx ) =>
    {
        var r = _cinit.Row = _idx / 5;
        var c = _cinit.Column = _idx % 5;
        _cinit.CardId = GetCardId( r, c );
        _cinit.Sprite = CardImages.GetSprite( _cinit.CardId );
    } );

    public int GetCardId( int r, int c )
    {
        var cards = RowsOfCardsSeparatedBySpaces[r];
        var listCards = cards.Split( ' ' ).Select( s => PlayingCard.IdFrom2String( s ) );
        return listCards.ElementAt( c );
    }
}

