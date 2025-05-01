using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MyMainScene");
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
        SceneManager.LoadScene("MyMainScene");
    }

    public void GoToEnd()
    {
        SceneManager.LoadScene("EndScreen");
    }
}