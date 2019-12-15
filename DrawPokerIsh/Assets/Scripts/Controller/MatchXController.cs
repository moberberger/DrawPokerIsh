using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Morpheus;
using System;
using System.Linq;




public class MatchXController : MonoBehaviour
{
    private CardController[] m_cards;

    // Start is called before the first frame update
    void Awake()
    {
        m_cards = ReflectionExtenstions.CreatePopulatedArray<CardController>( 25 );

        foreach (var c in m_cards)
            c.CardId = Rng.Default.Next( 52 );
    }

    // Update is called once per frame
    void Update()
    {

    }


    [AEventHandler]
    public void OnReqCardController( RequestCardController req )
    {
        //Debug.Log( $"ROW: {req.RowIndex}    COL: {req.ColumnIndex}" );
        req.CardController = m_cards[req.RowIndex * 5 + req.ColumnIndex];
    }
}
