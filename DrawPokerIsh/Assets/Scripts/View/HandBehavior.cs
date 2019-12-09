using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Morpheus; 

public class HandBehavior : MonoBehaviour, IPointerClickHandler
{
    public int Index;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( $"Hand {Index} Clicked" );
        Dispatcher.Default.Post( new HandClicked( Index ) );
    }
}
