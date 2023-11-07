using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private int waveNo;
    [SerializeField] GameObject upgradeManager;
    void Start()
    {
        waveNo = 0;
    }

    public void checkCompletion()
    {
        // will also have to check that there are no more to be spawned
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            upgradeManager.SetActive(true);
        }
    }
}
