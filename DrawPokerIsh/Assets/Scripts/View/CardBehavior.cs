using System.Collections;
using System.Collections.Generic;
using Morpheus;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

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
    public Image CardImage;

    public Image HeldImage;


    void Start()
    {
        HeldImage.enabled = false;
    }

    //public void OnPointerClick( PointerEventData eventData )
    //{
    //    Debug.Log( $"Clicked Card[{Index}]" );
    //}

    [AEventHandler]
    public void OnDisplayCard( CardDisplayRequest _evt )
    {
        //if (_evt.IsMatch( HandIndex, Index ))
        if (_evt.HandIndex == HandIndex && _evt.CardIndex == Index)
        {
            var spriteRequest = new CardSpriteRequest( _evt.CardId );
            Dispatcher.Default.Post( spriteRequest );
            if (CardImage.sprite != spriteRequest.Sprite)
            {
                CardImage.sprite = spriteRequest.Sprite;
                Debug.Log( $"New card at [{HandIndex}][{Index}]" );
            }
            else
            {
                Debug.Log( $"Using cached card at [{HandIndex}][{Index}]" );
            }
        }
    }

    [AEventHandler]
    public void OnHeldCards( SetHeldCards cards )
    {
        HeldImage.enabled = cards.Indicies.Contains( Index );
    }
}
