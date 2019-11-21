using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Morpheus;

public class PaylineClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Payline Payline { get; set; }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( $"Payline {Payline.Name} Clicked" );
        Payline.Prize *= 2;
        Dispatcher.Default.Post( new PaylineClickedMessage() { Payline = Payline } );
    }
}
