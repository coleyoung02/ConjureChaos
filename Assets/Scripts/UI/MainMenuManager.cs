using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void Start()
    {
        FindObjectOfType<AudioManager>().PlayMusic();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadWin()
    {
        SceneManager.LoadSceneAsync("WinScreen");
    }
}
