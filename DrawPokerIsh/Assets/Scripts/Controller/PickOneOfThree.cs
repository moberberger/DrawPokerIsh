using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Morpheus;


public class PickOneOfThree : MonoBehaviour
{
    private GameObject[] m_siblings;
    public int SelectedHandIndex = -1;

    private DeckOfCards m_deck = new DeckOfCards( 52 );
    private int[][] m_hands;


    // Start is called before the first frame update
    void Start()
    {
        m_hands = new int[5][];

        m_siblings = transform.parent.GetComponentsInChildren<HandBehavior>().Select( _beh => _beh.gameObject ).ToArray();

        for (int hand = 0; hand < 5; hand++)
        {
            m_hands[hand] = new int[5];
            for (int card = 0; card < 5; card++)
            {
                int cardId = m_deck.GetRandomCard().CardId;
                m_hands[hand][card] = cardId;

                Dispatcher.Default.Post( new CardDisplayRequest( card, hand, cardId ), EDispatchMode.Batched );
            }
        }

    }


    [AEventHandler]
    public void OnHandClicked( HandClicked evt )
    {
        m_siblings[3].SetActive( true );
        m_siblings[4].SetActive( true );
        SelectedHandIndex = evt.Index;

        for (int hand = 0; hand < 5; hand++)
        {
            for (int card = 0; card < 5; card++)
            {
                int cardId = m_hands[SelectedHandIndex][card];
                m_hands[hand][card] = cardId;

                Dispatcher.Default.Post( new CardDisplayRequest( card, hand, cardId ), EDispatchMode.Batched );
            }
        }
    }
}
