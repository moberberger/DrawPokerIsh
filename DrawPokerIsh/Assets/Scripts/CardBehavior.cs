using System.Collections;
using System.Collections.Generic;
using Morpheus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The index of this card UI object (0-4 likely)
    /// </summary>
    public int Index;

    /// <summary>
    /// The image to set when the cards change
    /// </summary>
    private Image m_cardImage;


    void Start()
    {
        m_cardImage = GetComponent<Image>();
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( $"Clicked Card[{Index}]" );
        Dispatcher.Default.Post( new DrawCardRequest( Index ) );
    }


    [AEventHandler]
    public void OnNewPokerHand( NewPokerHandDealtMessage hand )
    {
        var spriteRequest = new CardSpriteRequest( Index );
        Dispatcher.Default.Post( spriteRequest );
        if (m_cardImage.sprite != spriteRequest.Sprite)
        {
            m_cardImage.sprite = spriteRequest.Sprite;
            Debug.Log( $"New Card at Index {Index}" );
        }
    }
}
