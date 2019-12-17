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
        m_cards = ReflectionExtenstions.CreatePopulatedArray<CardController>( 25 );
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                int idx = row * 5 + col;
                var card = m_cards[idx];
                card.Row = row;
                card.Column = col;
                card.CardIndex = idx;
            }
        }
    }


    private int m_finishedCount = 0;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        DevGrid.SetActive( false );

        foreach (var card in m_cards)
        {
            var go = card.GameObject = Instantiate( CardPrefab );
            var cb = card.CardBehavior = go.GetComponentInChildren<MatchXCardBehavior>();
            cb.RowIndex = card.Row;
            cb.ColumnIndex = card.Column;
            cb.CardImages = CardImages;
            cb.CardController = card;

            go.transform.SetParent( CardBoard.transform, false );
            go.transform.position = GetXY( card.Row - 5, card.Column );

            float time = 2 + (5 - (float) card.Row) / 5 + (float) Rng.Default.NextDouble() / 2f;
            var finalPos = GetXY( card.Row, card.Column );

            go.transform.DOMove( finalPos, time )
                .SetEase( Ease.OutBounce )
                .OnComplete( () =>
                {
                    if (++m_finishedCount == 25)
                    {
                        Dispatcher.PostDefault( "Find 4 of a Kind hands" );
                        m_acceptInput = true;
                    }
                } );
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
                msg = "Not Connected";
            }
        }

        Dispatcher.PostDefault( msg );
    }



    const int CARD_WIDTH = 165;
    const int CARD_HEIGHT = 242;
    const int SPACING = 15;
    public static float GetX( int column ) => SPACING / 2 + CARD_WIDTH / 2 + column * (SPACING + CARD_WIDTH);
    public static float GetY( int row ) => 1300 - (SPACING + CARD_HEIGHT / 2 + row * (SPACING + CARD_HEIGHT));
    public static Vector3 GetXY( int row, int col ) => new Vector3( GetX( col ), GetY( row ) );



}
