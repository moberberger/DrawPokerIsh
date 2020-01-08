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

using DG.Tweening;

public class CardController
{
    public MatchXCardBehavior CardBehavior;
    public int CardIndex;
    public int Row;
    public int Column;

    public bool IsSelected;
    public bool IsUpsideDown;
    public string Message;
    public int CardId;

    public GameObject GameObject;


    public CardController( int index )
    {
        Row = index / 5;
        Column = index % 5;
        CardIndex = index;
    }

    public void InitializeGameObject( DeckOfCardsController CardImages, GameObject CardBoard )
    {
        var CardBehavior = GameObject.GetComponentInChildren<MatchXCardBehavior>();
        CardBehavior.RowIndex = Row;
        CardBehavior.ColumnIndex = Column;
        CardBehavior.CardImages = CardImages;
        CardBehavior.CardController = this; ;
        GameObject.transform.SetParent( CardBoard.transform, false );
    }


    const int CARD_WIDTH = 165;
    const int CARD_HEIGHT = 242;
    const int SPACING = 15;

    public static float GetX( int column ) => SPACING / 2 + CARD_WIDTH / 2 + column * (SPACING + CARD_WIDTH);
    public static float GetY( int row ) => 1300 - (SPACING + CARD_HEIGHT / 2 + row * (SPACING + CARD_HEIGHT));
    public static Vector3 GetXY( int row, int col ) => new Vector3( GetX( col ), GetY( row ) );


    public Tween DropCard( int initialRow, int finalRow, TweenCallback onComplete = null, Ease easeFn = Ease.OutBounce )
    {
        var initialPos = GetXY( initialRow, Column );
        var finalPos = GetXY( finalRow, Column );

        const float minTime = 1;
        float lerpTime = (finalRow - initialRow) / 5f;
        const float randomScale = 0.5f;

        float time = minTime + lerpTime + (float) Rng.Default.NextDouble() * randomScale;

        GameObject.transform.position = initialPos;
        var tween = GameObject.transform.DOMove( finalPos, time ).SetEase( easeFn );
        if (onComplete != null)
            tween.OnComplete( onComplete );

        return tween;
    }

}
