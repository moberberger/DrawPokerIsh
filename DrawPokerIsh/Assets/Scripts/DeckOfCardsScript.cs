
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using DG;
using DG.Tweening;
using DG.Tweening.Core;

public static class PlayingCard_Ext
{
}

public class DeckOfCardsScript : MonoBehaviour
{
    public Sprite[] CardsInDeckInOrder;

    private DeckOfCards m_deck = new DeckOfCards( 52 );

    public int GetNextRandomCard() => m_deck.GetRandomCard().CardId;
    public void Shuffle() => m_deck.Shuffle();

    private void Awake()
    {
    }
}

