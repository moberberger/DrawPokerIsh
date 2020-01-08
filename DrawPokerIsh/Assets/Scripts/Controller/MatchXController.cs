using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Morpheus;
using System;
using System.Linq;

using DG.Tweening;




public class MatchXController : MonoBehaviour
{
    private CardController[] m_cards;

    public GameObject CardPrefab;
    public GameObject CardBoard;
    public GameObject DevGrid;
    public DeckOfCardsController CardImages;

    private bool m_acceptInput = false;


    // Start is called before the first frame update
    void Awake()
    {
        m_cards = ReflectionExtenstions.CreatePopulatedArray<CardController>( 25, index => new CardController( index ) );
    }


    private int m_finishedCount = 0;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        DevGrid.SetActive( false );

        foreach (var card in m_cards)
        {
            card.GameObject = Instantiate( CardPrefab );
            card.InitializeGameObject( CardImages, CardBoard );

            TweenCallback enableInpotWhenAllComplete = () =>
            {
                if (++m_finishedCount == 25)
                {
                    Dispatcher.PostDefault( "Find 4 of a Kind hands" );
                    m_acceptInput = true;
                }
            };

            card.DropCard( card.Row - 5, card.Row, enableInpotWhenAllComplete );
        }

        yield return null;
    }


    [AEventHandler]
    public void InitCard( SetInitialCard cmd )
    {
        m_cards[cmd.CardIndex].CardId = cmd.CardId;
    }

    [AEventHandler]
    public void OnCardClicked( CardClicked evt )
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
            }
            else
            {
                msg = "Card Not Connected";
            }
        }

        Dispatcher.PostDefault( msg );
    }



}
