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

public class MatchXCardBehavior : MonoBehaviour, IPointerClickHandler
{
    public int RowIndex;
    public int ColumnIndex;
    public string InitialCard2Char;

    private CardController m_card;
    private bool m_dirty = true;

    public Image CardImage;
    public TextMeshProUGUI MessageObject;
    public Image SelectedImage;

    // Start is called before the first frame update
    void Start()
    {
        var req = new RequestCardController( RowIndex, ColumnIndex );
        Dispatcher.PostDefault( req );
        m_card = req.CardController;

        var card = PlayingCard.From2String( InitialCard2Char );
        if (card != null)
        {
            m_card.CardId = card.CardId;
        }
    }

    public void OnPropChanged( object _object, string _propertyName, object _previousValue, object _newValue )
    {
        Debug.Log( $"{_propertyName}" );
        m_dirty = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dirty)
        {
            var id = m_card.IsUpsideDown ? -1 : (int) m_card.CardId;
            var req = new CardSpriteRequest( id );
            Dispatcher.Default.Post( req );

            CardImage.sprite = req.Sprite;
            MessageObject.text = m_card.Message;
            SelectedImage.enabled = m_card.IsSelected;

            m_dirty = false;
        }
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( $"CLICKED: ROW: {RowIndex}    COL: {ColumnIndex}" );

        m_card.IsSelected = !m_card.IsSelected;
        m_card.Message = "BARF";

        m_dirty = true;
    }
}
