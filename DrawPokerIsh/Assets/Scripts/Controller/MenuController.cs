using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GotoMainMenu()
    {
        SceneManager.LoadScene( "Main Menu" );
    }


    public void GotoPickX()
    {
        SceneManager.LoadScene( "PickOneHand" );
    }

    public void GotoMatchX()
    {
        SceneManager.LoadScene( "MatchX" );
    }

    public void GotoWinWheels()
    {
        SceneManager.LoadScene( "Win Wheels" );
    }
}
