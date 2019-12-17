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
public class MatchXInitialCardScript : MonoBehaviour
{
    public int RowIndex;
    public int ColumnIndex;
    public string InitialCard2Char;

    public DeckOfCardsController CardImages;
    public Image CardImage;

    private int CardId => PlayingCard.IdFrom2String( InitialCard2Char );

    void Start()
    {
        CardImage = GetComponentInChildren<Image>();

        if (Application.isPlaying)
        {
            var post = new SetInitialCard( RowIndex, ColumnIndex, CardId );
            Dispatcher.PostDefault( post );
        }
    }


    void Update()
    {
        if (!Application.isPlaying)
        {
            var sprite = CardImages.GetSprite( CardId );
            CardImage.sprite = sprite;
        }
    }

}
