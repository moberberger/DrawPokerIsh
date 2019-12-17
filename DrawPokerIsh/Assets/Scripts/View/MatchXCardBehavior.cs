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
    private bool m_dirty = true;

    public CardController CardController;
    public DeckOfCardsController CardImages;

    public int RowIndex;
    public int ColumnIndex;

    // These come from the Prefab (assuming)
    public Image CardImage;
    public TextMeshProUGUI MessageObject;
    public Image SelectedImage;


    void Update()
    {
        if (!m_dirty) return;

        var id = CardController.IsUpsideDown ? -1 : (int) CardController.CardId;
        CardImage.sprite = CardImages.GetSprite( id );
        MessageObject.text = CardController.Message;
        SelectedImage.enabled = CardController.IsSelected;

        m_dirty = false;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        m_dirty = true;

        Dispatcher.PostDefault( new CardClicked( CardController ) );
    }
}
