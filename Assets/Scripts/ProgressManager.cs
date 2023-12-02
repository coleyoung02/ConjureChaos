using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public int enemiesDefeated;
    [SerializeField] private Slider deathCountBar;
    public WaveManager waveMan;
    public int currKillQuota;
    [SerializeField] GameObject upgradeManager;

    public float target;
    void Start()
    {
        Enemy.deathListenerAdded = false;
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
        if (deathCountBar.value < target)
        {
            deathCountBar.value = Mathf.Min(target, deathCountBar.value + Time.deltaTime * .5f);
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
            upgradeManager.GetComponent<UpgradeManager>().GetUpgrades();
        }
    }

    public void incrementDeathCounter()
    {
        enemiesDefeated++;
        UpdateDeathCountUI();
    }

    void UpdateDeathCountUI()
    {
        if (currKillQuota <= 0)
        {
            deathCountBar.value = 0f;
        }
        else
        {
            target = enemiesDefeated / (float)currKillQuota;
            if (target < deathCountBar.value)
            {
                deathCountBar.value = 0f;
            }
        }
    }
}
