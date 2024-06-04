using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void EndGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        FindAnyObjectByType<AudioManager>().SetFilter(false);
        SceneManager.LoadScene("MainMenu");
    }
}
