
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

    public int GetNextRandomCard() => m_deck.GetRandomCard().CardId;
    public void Shuffle() => m_deck.Shuffle();

    IEnumerator Start()
    {
        // cards have to have their Images
        yield return new WaitForEndOfFrame();

        var hand = new PokerHand()
        {
            Cards = m_deck.GetRandomCards( 5 ).ToArray()
        };
        Dispatcher.Default.Post( hand );

        yield return null;
    }
}

