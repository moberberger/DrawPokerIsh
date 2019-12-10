using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Morpheus;


public class MessageTextBehavior : MonoBehaviour
{
    public TextMeshProUGUI MessageText;

    // Start is called before the first frame update
    void Start()
    {
        MessageText.text = "";
    }


    [AEventHandler]
    public void OnNewText( string message ) => MessageText.text = message;
}
