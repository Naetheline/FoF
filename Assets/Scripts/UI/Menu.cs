using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void OnclickNewGame()
    {
        // Maybe play sound

        // Generate a seed for the entire game.

        // FIXME not working at all !
        SceneManager.LoadScene(1);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(0);

    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
