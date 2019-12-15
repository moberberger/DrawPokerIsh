using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Morpheus;

public class OnResetButtonClicked : MonoBehaviour
{
    public void OnClick()
    {
        Dispatcher.Default.Post( new ResetGameMessage() );
    }
}
