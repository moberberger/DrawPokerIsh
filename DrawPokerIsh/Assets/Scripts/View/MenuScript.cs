using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void OnPickX()
    {
        SceneManager.LoadScene( "PickOneHand" );
    }

    public void OnMatchX()
    {
        SceneManager.LoadScene( "MatchX" );
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene( "Main Menu" );
    }
}
