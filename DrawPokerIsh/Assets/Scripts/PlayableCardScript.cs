using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class PlayableCardScript : MonoBehaviour, IPointerClickHandler
{
    public DeckOfCardsScript Deck;
    public PlayerHandScript Hand;
    public const int BackCardIndex = 52;
    public int PlayingCardIndex;

    private int m_lastIndex = BackCardIndex;
    private Image m_cardImage;

    public Sprite Sprite
    {
        get { return m_cardImage.sprite; }
        set { m_cardImage.sprite = value; }
    }

    // Use this for initialization
    void Start()
    {
        m_cardImage = GetComponent<Image>();
        SetSpriteForCardIndex();
    }

    private void SetSpriteForCardIndex( int index = BackCardIndex )
    {
        m_lastIndex = (index < 0 || index >= BackCardIndex) ? BackCardIndex : index;
        Sprite = Deck.CardsInDeckInOrder[m_lastIndex];

    }

    public void SetCardId( int cardId ) => SetSpriteForCardIndex( cardId );
    public bool IsCardBack => m_lastIndex == BackCardIndex;




    public void OnPointerClick( PointerEventData eventData )
    {
        if (m_lastIndex == BackCardIndex)
        {
            SetSpriteForCardIndex( Deck.GetNextRandomCard() );
        }
        else
        {
            Hand.FlyCard( m_cardImage );
            SetSpriteForCardIndex( Deck.GetNextRandomCard() );
        }
    }
}
