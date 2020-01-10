using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Morpheus;
using System;
using System.Linq;

using DG.Tweening;




public class MatchXController : MonoBehaviour
{
    private MatchXCardBehavior[] m_cards;
    public GameObject CardPrefab;
    public GameObject CardBoard;
    public GameObject DevGrid;
    public DeckOfCardsController CardImages;

    private bool m_acceptInput = false;
    private int m_finishedCount = 0;

    GDI get these two fucking classes to work together. They should both be "view" centric

    void Start()
    {
        m_cards = GetComponentsInChildren<MatchXCardBehavior>();

        DevGrid.SetActive( false );

        foreach (var card in m_cards)
        {
            card.CardImages = CardImages;
            card.gameObject.transform.SetParent( this.transform );


            card.InitializeGameObject( CardImages, CardBoard );

            TweenCallback enableInputWhenAllComplete = () =>
            {
                if (++m_finishedCount == 25)
                {
                    Dispatcher.PostDefault( "Find 4 of a Kind hands" );
                    m_acceptInput = true;
                }
            };

            card.DropCard( card.Row - 5, card.Row, enableInputWhenAllComplete );
        }
    }


    [AEventHandler]
    public void InitCard( SetInitialCard cmd )
    {
        m_cards[cmd.CardIndex].CardId = cmd.CardId;
    }

    [AEventHandler]
    public void OnCardClicked( MatchXCardClicked evt )
    {
        if (!m_acceptInput) return;

        // Debug.Log( $"Card Clicked: {evt.Card.Row} / {evt.Card.Column}" );

        var selectedCards = m_cards.Where( c => c.IsSelected ).ToList();
        var msg = "";

        if (selectedCards.Count == 0)
        {
            evt.Card.IsSelected = true;
        }
        else if (selectedCards.Contains( evt.Card ))
        {
            evt.Card.IsSelected = false;
        }
        else
        {
            if (selectedCards[0].CardId >> 2 != evt.Card.CardId >> 2)
            {
                msg = "Wrong Rank";
            }
            else if (selectedCards.Contains( c =>
                {
                    var rowDiff = Math.Abs( c.Row - evt.Card.Row );
                    var colDiff = Math.Abs( c.Column - evt.Card.Column );
                    return (rowDiff == 0 && colDiff == 1) ||
                           (colDiff == 0 && rowDiff == 1);
                } ))
            {
                evt.Card.IsSelected = true;

                if (selectedCards.Count == 3) // it would now be 4
                {
                    msg = "Found One!";
                    HandleFoundHand();
                }
            }
            else
            {
                msg = "Card Not Connected";
            }
        }

        Dispatcher.PostDefault( msg );
    }

    private void HandleFoundHand()
    {
        var byCol = m_cards.GroupBy( c => c.Column );
        Debug.Log( byCol.Select( g => g.Key ).JoinAsString( ", " ) );

        foreach (var columnOfCards in byCol)
        {
            Debug.Log( $"Column: {columnOfCards.Key}" );
            Debug.Log( $" Cards: {columnOfCards.Select( c => c.Row ).JoinAsString( ", " )}" );

            HandleColumn( columnOfCards );
        }
    }

    private void HandleColumn( IEnumerable<CardController> cardsInColumn )
    {
        var cards = new CardController[5];

        foreach (var c in cardsInColumn) cards[c.Row] = c;

        int selectedCount = 0;
        for (int i = cards.Length - 1; i >= 0; i--)
        {
            var card = cards[i];
            card.Message = "";

            if (card.IsSelected)
            {
                selectedCount++;
            }
            else
            {
                card.SetRowWithAnimation( card.Row + selectedCount );
            }
        }

        cards.Where( c => c.IsSelected )
            .ForEach( ( c, idx ) => c.RecycleCard( idx ) );
    }
}
