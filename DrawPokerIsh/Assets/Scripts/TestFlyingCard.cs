using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Morpheus;
using System;
using System.Linq;

using DG.Tweening;



public class TestFlyingCard : MonoBehaviour
{
    const int CARD_WIDTH = 165;
    const int CARD_HEIGHT = 242;
    const int SPACING = 15;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GetXY( 0, 0 );
        transform.DOMove( GetXY( 4, 4 ), 5 )
            .SetEase( Ease.InOutBounce )
            ;//.OnComplete( () => gameObject.SetActive( false ) );
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetX( int column ) => SPACING + CARD_WIDTH / 2 + column * (SPACING + CARD_WIDTH);
    public float GetY( int row ) => 1300 - (SPACING + CARD_HEIGHT / 2 + row * (SPACING + CARD_HEIGHT));
    public Vector3 GetXY( int row, int col ) => new Vector3( GetX( col ), GetY( row ) );
}
