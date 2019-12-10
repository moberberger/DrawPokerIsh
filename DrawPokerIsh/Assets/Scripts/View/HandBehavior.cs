using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Morpheus;
using Protobuf.Cards;
using TMPro;

public class HandBehavior : MonoBehaviour, IPointerClickHandler
{
    public int Index;

    public TextMeshProUGUI WinText;

    public string MessageText;


    /// <summary>
    /// 2-char comma separated list of cards to initially populate these slots
    /// </summary>
    public string StartingHand;

    /// <summary>
    /// THESE ARE 1-BASED INDICIES! Zero indicates an unused index. Only
    /// positive indicies will be used.
    /// </summary>
    public int[] HeldCardIndicies = new int[5];

    /// <summary>
    /// If this hand is selected, then these are the next hands to be delivered
    /// to all other slots
    /// </summary>
    public string[] NextHands = new string[5];

    public string[] NextHandWinTexts = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        foreach (var card in GetComponentsInChildren<CardBehavior>())
            card.HandIndex = Index;

        HeldCardIndicies = HeldCardIndicies.Where( x => x > 0 ).Select( x => x - 1 ).ToArray();
        WinText.text = "";

        Dispatcher.Default.Post( new SetInitialHand( Index, StartingHand, "" ) );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( $"Hand {Index} Clicked" );
        Dispatcher.Default.Post( new HandClicked( Index ) );

        // VERY DOMAIN DRIVEN! Refactor
        for (int hand = 0; hand < 5; hand++)
        {
            Dispatcher.Default.Post( new SetInitialHand( hand, NextHands[hand], NextHandWinTexts[hand] ) );
        }

        Dispatcher.Default.Post( new SetHeldCards( HeldCardIndicies ) );

        Dispatcher.Default.Post( MessageText );
    }

    [AEventHandler]
    public void OnHandEstablished( SetInitialHand hand )
    {
        if (hand.Index == Index)
        {
            WinText.text = hand.WinText;
            for (int card = 0; card < 5; card++)
            {
                Dispatcher.Default.Post( new CardDisplayRequest( card, Index, hand.Cards[card].CardId ) );
            }
        }
    }
}
