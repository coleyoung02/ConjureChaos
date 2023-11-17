using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressManager : MonoBehaviour
{
    private int enemiesDefeated;
    public TextMeshProUGUI deathCountText;
    private int waveNo;
    [SerializeField] GameObject upgradeManager;
    void Start()
    {
        enemiesDefeated = 0;
        UpdateDeathCountUI();
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

    public void incrementDeathCounter()
    {
        enemiesDefeated++;
        UpdateDeathCountUI();
    }

    void UpdateDeathCountUI()
    {
        deathCountText.text = $"{enemiesDefeated} Enemies Defeated";
    }
}
