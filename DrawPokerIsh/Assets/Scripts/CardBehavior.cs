using System.Collections;
using System.Collections.Generic;
using Morpheus;
using UnityEngine;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour
{
    /// <summary>
    /// The index of this card UI object (0-4 likely)
    /// </summary>
    public int Index;

    /// <summary>
    /// Where to get the images from, along with "deck" functionality
    /// </summary>
    public DeckOfCardsScript DeckOfCards;

    /// <summary>
    /// The image to set when the cards change
    /// </summary>
    private Image m_cardImage;


    void Awake()
    {
        new MessageHandlerDiscovery().RegisterInstanceHandlers( this );
    }

    void Start()
    {
        m_cardImage = GetComponent<Image>();
    }


    [AEventHandler]
    public void OnNewPokerHand( PokerHand hand )
    {
        Debug.Log( "Received PokerHand" );
        m_cardImage.sprite = DeckOfCards.CardsInDeckInOrder[hand.Cards[Index].CardId];
    }
}
