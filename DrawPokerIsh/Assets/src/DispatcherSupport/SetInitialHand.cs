using System;
using System.Collections.Generic;
using System.Linq;
using Morpheus;
using Protobuf.Cards;

public class SetInitialHand
{
    public int Index;
    public string StartingHand;
    public string WinText;

    public IList<PlayingCard> Cards;


    public SetInitialHand( int index, string startingHand, string winText )
    {
        this.Index = index;
        this.StartingHand = startingHand;
        this.WinText = winText ?? "";

        var shand = StartingHand.Split( ',' );
        if (shand.Length != 5)
            throw new ArgumentException( "Must contain exactly 5x 2-char card identifiers separated by commas." );

        Cards = shand.Select( _c => PlayingCard.From2String( _c.RemoveDuplicateWhitespace() ) ).ToList();
    }
}