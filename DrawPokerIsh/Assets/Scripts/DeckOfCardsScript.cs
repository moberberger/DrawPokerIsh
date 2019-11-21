
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG;
using DG.Tweening;
using DG.Tweening.Core;
using Morpheus;
using System.Collections;

public class DeckOfCardsScript : MonoBehaviour
{
    public Sprite[] CardsInDeckInOrder;

    private DeckOfCards m_deck = new DeckOfCards( 52 );

    private PlayingCard[] m_hand = new PlayingCard[5];

    IEnumerator Start()
    {
        // cards have to have their Images
        yield return new WaitForEndOfFrame();

        m_hand = m_deck.GetRandomCards( 5 ).ToArray();

        var hand = new NewPokerHandDealtMessage()
        {
            Cards = m_hand
        };
        Dispatcher.Default.Post( hand );

        yield return null;
    }

    [AEventHandler]
    public void OnDrawCardRequest( DrawCardRequest _req )
    {
        var card = m_deck.GetRandomCard();
        m_hand[_req.Index] = card;
        var hand = new NewPokerHandDealtMessage()
        {
            Cards = m_hand
        };
        Dispatcher.Default.Post( hand );
    }
}

