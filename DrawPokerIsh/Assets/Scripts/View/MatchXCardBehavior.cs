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
public class MatchXCardBehavior : MonoBehaviour, IPointerClickHandler
{
    private CardController m_card;
    private bool m_dirty = true;
    private int m_stage;

    public int RowIndex;
    public int ColumnIndex;
    public string InitialCard2Char;
    public int AvailableStage = 0;



    public DeckOfCardsController CardImages;
    public Image CardImage;
    public TextMeshProUGUI MessageObject;
    public Image SelectedImage;



    private bool IsValidStage() => (m_stage & AvailableStage) != 0;



    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            var req = new RequestCardController( RowIndex, ColumnIndex );
            Dispatcher.PostDefault( req );
            m_card = req.CardController;
            m_card.CardBehavior = this;

            var card = PlayingCard.From2String( InitialCard2Char );
            if (card != null && m_card != null)
            {
                m_card.CardId = card.CardId;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dirty && !Application.isPlaying)
        {
            var cardId = PlayingCard.IdFrom2String( InitialCard2Char );
            var sprite = CardImages.GetSprite( cardId );
            CardImage.sprite = sprite;
        }
        else if (m_dirty)
        {
            var id = m_card.IsUpsideDown ? -1 : (int) m_card.CardId;
            var req = new CardSpriteRequest( id );
            Dispatcher.Default.Post( req );

            CardImage.sprite = req.Sprite;
            MessageObject.text = m_card.Message;
            SelectedImage.enabled = m_card.IsSelected;

            if (IsValidStage())
                CardImage.color = new Color( 0.6f, 0.75f, 1, 1 );
            else
                CardImage.color = new Color( 1, 1, 1, 1 );

            m_dirty = false;
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        if (!IsValidStage())
        {
            Dispatcher.PostDefault( "Invalid Selection" );
            return;
        }

        m_card.IsSelected = !m_card.IsSelected;
        m_dirty = true;
        Dispatcher.PostDefault( "" );
        Dispatcher.PostDefault( new CardClicked( m_card ) );
    }

    [AEventHandler]
    public void OnStageSet( int stage )
    {
        m_stage = stage;
        m_dirty = true;
    }
}
