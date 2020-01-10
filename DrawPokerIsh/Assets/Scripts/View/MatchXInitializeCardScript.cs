using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Morpheus;
using TMPro;
using UnityEngine.EventSystems;
using Protobuf.Cards;

[ExecuteInEditMode]
public class MatchXInitializeCardScript : MonoBehaviour
{
    public int Row;
    public int Column;
    public int CardId;
    public Sprite Sprite;

    public Image CardImage;

    void Update()
    {
        if (!Application.isPlaying)
        {
            CardImage.sprite = Sprite;
        }
    }
}
