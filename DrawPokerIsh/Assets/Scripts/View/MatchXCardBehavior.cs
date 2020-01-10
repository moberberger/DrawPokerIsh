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



public class MatchXCardBehavior : MonoBehaviour, IPointerClickHandler
{
    private bool m_dirty = false;

    public int CardId = -1;
    public int Row = -1;
    public int Column = -1;
    public bool IsSelected = false;
    public bool IsUpsideDown = false;
    public string Message = "";

    public DeckOfCardsController CardImages;


    // These come from the Prefab (assuming)
    public Image CardImage;
    public Image SelectedImage;
    public TextMeshProUGUI MessageObject;


    void Update()
    {
        if (m_dirty)
        {
            var id = IsUpsideDown ? -1 : (int) CardId;
            var sprite = CardImages.GetSprite( id );
            CardImage.sprite = sprite;
            MessageObject.text = Message;
            SelectedImage.enabled = IsSelected;

            //Debug.Log( $"Update: [{Row},{Column}] = {CardId}" );

            m_dirty = false;
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Dispatcher.PostDefault( new MatchXCardClicked( this ) );
        m_dirty = true;
    }

    public void ResetCard( int _cardId, int _row, int _column )
    {
        Row = _row;
        Column = _column;
        SetCardId( _cardId );

        //Debug.Log( $"Reset: [{Row},{Column}] = {CardId}" );
    }


    public void SetCardId( int cid )
    {
        CardId = cid;
        IsSelected = false;
        IsUpsideDown = false;
        Message = "";
        m_dirty = true;

        //Debug.Log( $"SetCardId: [{Row},{Column}] = {CardId}" );
    }

    public void SetRowWithAnimation( int newRow )
    {
        if (newRow != Row)
        {
            int curRow = Row;
            Row = newRow;
            DropCard( curRow, newRow );
            m_dirty = true;
        }
    }

    public void RecycleCard( int newRow, int cardId )
    {
        Row = newRow;
        DropCard( newRow - 5, newRow );
        IsSelected = false;
        IsUpsideDown = false;
        Message = "*";
        CardId = cardId;
        m_dirty = true;

        // Debug.Log( $"Recycle: [{Row},{Column}] = {CardId}" );
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

        transform.position = initialPos;
        var tween = transform.DOMove( finalPos, time ).SetEase( easeFn );
        if (onComplete != null)
            tween.OnComplete( onComplete );

        return tween;
    }
}
