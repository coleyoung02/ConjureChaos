using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressManager : MonoBehaviour
{
    public int enemiesDefeated;
    public TextMeshProUGUI deathCountText;
    public WaveManager waveMan;
    public int currKillQuota;
    [SerializeField] GameObject upgradeManager;
    void Start()
    {
        enemiesDefeated = 0;
        currKillQuota = waveMan.killQuota;
        UpdateDeathCountUI();
    }

    void Update()
    {
        if(currKillQuota <= 0)
        {
            currKillQuota = waveMan.killQuota;
            UpdateDeathCountUI();
        }
    }

    public void checkCompletion()
    {
        // will also have to check that there are no more to be spawned
        if (enemiesDefeated >= currKillQuota)
        {
            waveMan.nextWave();
            enemiesDefeated = 0;
            currKillQuota = waveMan.killQuota;
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
        deathCountText.text = $"{enemiesDefeated} Enemies Defeated of {currKillQuota}";
    }
}
