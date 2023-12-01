using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;
    private bool upgrading;

    private void Start()
    {
        if(pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            if (upgrading)
            {
                upgradeMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        else
        {
            pauseMenu.SetActive(true);
            if (upgradeMenu.activeSelf)
            {
                upgradeMenu.SetActive(false);
                upgrading = true;
            }
            else
            {
                Time.timeScale = 0;
                upgrading = false;
            }
        }
    }
}
