using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Morpheus;
using Protobuf.Cards;

using DG.Tweening;

namespace fuckit
{
    public class CardController
    {
        public MatchXCardBehavior CardBehavior;
        public int Row;
        public int Column;

        public bool IsSelected;
        public bool IsUpsideDown;
        public string Message;
        public int CardId;

        public GameObject GameObject;


        public CardController( int index )
        {
            Row = index / 5;
            Column = index % 5;
        }

        public void InitializeGameObject( DeckOfCardsController CardImages, GameObject CardBoard )
        {
            var CardBehavior = GameObject.GetComponent<MatchXCardBehavior>();
            if (CardBehavior == null) throw new InvalidOperationException( "The CardBehavior must be set" );

            CardBehavior.CardImages = CardImages;
            CardBehavior.CardController = this; ;

            // !!!!!!!!! !! !
            GameObject.transform.SetParent( CardBoard.transform, false );
        }

        public void RecycleCard( int newRow )
        {
            Row = newRow;
            DropCard( newRow - 5, newRow );
            IsSelected = false;
            IsUpsideDown = false;
            Message = "New";
            CardId = Rng.Default.Next( 52 );

            CardBehavior.RecycleCard();
        }

        public void SetRowWithAnimation( int newRow )
        {
            if (newRow != Row)
            {
                int curRow = Row;
                Row = newRow;
                DropCard( curRow, newRow );
            }
        }

    }
}