using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Morpheus;
using System;
using System.Linq;




public class MatchXController : MonoBehaviour
{
    private CardController[] m_cards;
    public int Stage = 1;

    // Start is called before the first frame update
    void Awake()
    {
        m_cards = ReflectionExtenstions.CreatePopulatedArray<CardController>( 25 );

        for (int i = 0; i < m_cards.Length; i++)
        {
            var c = m_cards[i];
            c.CardId = Rng.Default.Next( 52 );
            c.CardIndex = i;
        }
    }

    void Start()
    {
        Dispatcher.PostDefault( Stage );
    }

    [AEventHandler]
    public void OnReqCardController( RequestCardController req )
    {
        //Debug.Log( $"ROW: {req.RowIndex}    COL: {req.ColumnIndex}" );
        req.CardController = m_cards[req.RowIndex * 5 + req.ColumnIndex];
    }

    [AEventHandler]
    public void OnCardClicked( CardClicked evt )
    {
        if (m_cards.Count( c => c.IsSelected ) == 4)
        {
            Dispatcher.PostDefault( "4 of a Kind = 25" );
            foreach (var c in m_cards.Where( c => c.IsSelected ))
            {
                for (var row = c.CardBehavior.RowIndex; row > 0; row--)
                {
                    int idx = row * 5 + c.CardBehavior.ColumnIndex;
                    int idxAbove = idx - 5;
                    m_cards[idx].CardId = m_cards[idxAbove].CardId;
                }
                m_cards[c.CardBehavior.ColumnIndex].CardId = -1;
                c.IsSelected = false;
            }

            Stage *= 2;
            Dispatcher.PostDefault( Stage );
        }
    }
}
