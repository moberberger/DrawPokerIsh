
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG;
using DG.Tweening;
using DG.Tweening.Core;



public class PlayerHandScript : MonoBehaviour
{
    public PlayableCardScript[] Cards;
    public DeckOfCardsScript Deck;
    public GameObject FlyawayObject;
    public GameObject Discards;

    void Start()
    {
        Cards = Cards.OrderBy( c => c.transform.position.x ).ToArray();
        Discards.transform.position = FlyawayObject.transform.position;
    }

    public void StartGame()
    {
        Deck.Shuffle();
        foreach (var card in Cards)
            card.SetCardId( -1 );
        foreach (Transform child in Discards.transform)
            DestroyObject( child );
    }

    public void FinishGame()
    {
    }


    public void FlyCard( Image cardImage )
    {
        var _time = 1f;

        var fly = Instantiate( FlyawayObject, Discards.transform );
        fly.transform.position = cardImage.transform.position;
        fly.transform.localScale = new Vector3( 2.25f, 2.25f, 1f );
        fly.GetComponent<Image>().sprite = cardImage.sprite;

        var t1 = fly.transform.DOMove( FlyawayObject.transform.position, _time ).SetEase( Ease.InOutSine );
        var t2 = fly.transform.DOScale( Vector3.one, _time );
    }

}
