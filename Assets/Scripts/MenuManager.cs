using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("build");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("build");
    }

    public void GoToEnd()
    {
        SceneManager.LoadScene("EndScreen");
    }
}