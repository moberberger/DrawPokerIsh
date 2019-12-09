using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CardSpriteRequest
{
    public int Index;
    public CardSpriteRequest( int _index ) => Index = _index;
    public Sprite Sprite { get; set; }
}
