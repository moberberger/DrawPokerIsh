using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Morpheus;
using Protobuf.DrawPoker;

public class PaylineClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Payline Payline { get; set; }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( $"Payline {Payline.EnglishDescription} @ {Payline.WinAmounts[0]} Clicked" );
        Payline.WinAmounts[0] *= 2;
        Dispatcher.Default.Post( new PaylineClickedMessage() { Payline = Payline } ); 
    }
}
