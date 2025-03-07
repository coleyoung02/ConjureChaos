using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject newParent;
    [SerializeField] private GameObject blocker;

    private bool waiting = false;
    public void StartGame()
    {
        if (!waiting)
        {
            waiting = true;
            StartCoroutine(LoadAfterFractionOfASecond());
        }
    }

    private IEnumerator LoadAfterFractionOfASecond()
    {
        blocker.SetActive(true);
        lights.transform.SetParent(newParent.transform, true);
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadSceneAsync("Main");
    }

    public void Start()
    {
        FindAnyObjectByType<AudioManager>().PlayMusic();
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
