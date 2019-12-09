using System.Collections;
using System.Collections.Generic;
using Morpheus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour//, IPointerClickHandler
{
    /// <summary>
    /// The index of this card UI object (0-4 likely)
    /// </summary>
    public int Index;

    /// <summary>
    /// Which hand does this card belong to
    /// </summary>
    public int HandIndex = -1;

    /// <summary>
    /// The image to set when the cards change
    /// </summary>
    private Image m_cardImage;


    void Start()
    {
        m_cardImage = GetComponent<Image>();

        var parent = GetComponentInParent<HandBehavior>();
        if (parent != null)
            HandIndex = parent.Index;
    }

    //    public void OnPointerClick( PointerEventData eventData )
    //    {
    //        Debug.Log( $"Clicked Card[{Index}]" );
    //        Dispatcher.Default.Post( new DrawCardRequest( Index ) );
    //    }

    [AEventHandler]
    public void OnDisplayCard( CardDisplayRequest _evt )
    {
        if (_evt.IsMatch( HandIndex, Index ))
        {
            var spriteRequest = new CardSpriteRequest( _evt.CardId );
            Dispatcher.Default.Post( spriteRequest );
            if (m_cardImage.sprite != spriteRequest.Sprite)
            {
                m_cardImage.sprite = spriteRequest.Sprite;
                Debug.Log( $"New card at [{HandIndex}][{Index}]" );
            }
            else
            {
                Debug.Log( $"Using cached card at [{HandIndex}][{Index}]" );
            }
        }
        else
        {
            Debug.Log( $"Skipping card at [{HandIndex}][{Index}]" );
        }
    }
}
