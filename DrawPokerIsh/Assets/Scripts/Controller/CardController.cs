using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Morpheus;
using UnityEngine;

public class CardController
{
    public MatchXCardBehavior CardBehavior;
    public int CardIndex;
    public int Row;
    public int Column;

    public bool IsSelected;
    public bool IsUpsideDown;
    public string Message;
    public int CardId;

    public GameObject GameObject;
}
