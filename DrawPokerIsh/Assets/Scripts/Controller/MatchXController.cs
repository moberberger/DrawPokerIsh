using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Morpheus;
using System;
using System.Linq;

using DG.Tweening;
using UnityEngine.UI;

public class MatchXController : MonoBehaviour
{
    public GameObject CardPrefab;
    public DeckOfCardsController CardImages;
    public GridLayoutGroup GridLayoutGroup;

    private MatchXCardBehavior[] m_cards;
    private bool m_acceptInput = false;
    private int m_finishedCount = 0;

    private DeckOfCards m_deck = new DeckOfCards( 52 );
    private string m_animationCompleteMessage = "";

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        GridLayoutGroup.enabled = false;

        m_cards = GetComponentsInChildren<MatchXCardBehavior>();
        Debug.LogWarning( "Count: " + m_cards.Count() );

        foreach (var card in m_cards)
        {
            card.CardImages = CardImages;

            var cardInit = card.GetComponent<MatchXInitializeCardScript>();
            card.ResetCard( cardInit.CardId, cardInit.Row, cardInit.Column );
            if (m_deck.RemoveCard( cardInit.CardId ))
                Debug.Log( $"Duplicate!: [{cardInit.Row},{cardInit.Column}] = {cardInit.CardId}" );

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

        yield return null;
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
                    return true;
                    //return rowDiff <= 1 && colDiff <= 1;
                    //return (rowDiff == 0 && colDiff == 1) ||
                    //       (colDiff == 0 && rowDiff == 1);
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

        foreach (var columnOfCards in byCol)
        {
            HandleColumn( columnOfCards );
        }

        ShowLargestSet();
    }

    private void ShowLargestSet()
    {
        var byRank = m_cards.Where( c => c.CardId >= 0 ).GroupBy( c => c.CardId >> 2 );
        var max = byRank.Max( g => g.Count() );

        var msg = $"Largest Set = {max} ... ";
        msg += byRank.Where( g => g.Count() == max ).Select( g => g.Key + 2 ).OrderBy( x => x ).JoinAsString( ", " );

        Debug.Log( msg );

        if (max < 4)
        {
            m_acceptInput = false;
            m_animationCompleteMessage = "No More Options";
            Dispatcher.Default.Post( m_animationCompleteMessage, EDispatchMode.Batched );
        }
    }

    private void HandleColumn( IEnumerable<MatchXCardBehavior> cardsInColumn )
    {
        var cards = new MatchXCardBehavior[5];

        foreach (var c in cardsInColumn) cards[c.Row] = c;

        int selectedCount = 0;
        for (int i = cards.Length - 1; i >= 0; i--)
        {
            var card = cards[i];

            if (card.IsSelected)
            {
                int newRow = selectedCount;

                selectedCount++;
                var newCard = m_deck.GetRandomCard();
                if (newCard == null)
                { // when we're out of cards, do something special or something.
                    Debug.LogWarning( "Out of cards" );
                    foreach (var c in m_cards) c.DropCard( c.Row - 1, c.Row );
                    m_acceptInput = false;
                    Dispatcher.PostDefault( "You Won!", EDispatchMode.Batched );
                }
                var id = (newCard == null) ? -1 : newCard.CardId;
                card.RecycleCard( newRow, id );
            }
            else
            {
                card.SetRowWithAnimation( card.Row + selectedCount );
            }
        }
    }

    public void RandomizeBoard()
    {
        m_deck.Shuffle();
        m_finishedCount = 0;
        m_acceptInput = true;
        Dispatcher.Default.Post( "" );
        m_animationCompleteMessage = "Find 4 of a Kind hands";

        foreach (var card in m_cards)
        {
            var cid = m_deck.GetRandomCard().CardId;
            card.SetCardId( cid );

            TweenCallback enableInputWhenAllComplete = () =>
            {
                if (++m_finishedCount == 25)
                {
                    Dispatcher.PostDefault( m_animationCompleteMessage );
                    m_acceptInput = true;
                }
            };

            card.DropCard( card.Row - 5, card.Row, enableInputWhenAllComplete );
        }

        ShowLargestSet();
    }
}
