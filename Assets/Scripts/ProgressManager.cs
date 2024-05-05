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
        if (waveMan != null)
        {
            currKillQuota = waveMan.GetKillQuota();
        }
        UpdateDeathCountUI();

    }

    void Update()
    {
        if(waveMan != null && currKillQuota <= 0)
        {
            currKillQuota = waveMan.GetKillQuota();
            UpdateDeathCountUI();
        }
        if (deathCountBar.value < target)
        {
            deathCountBar.value = Mathf.Min(target, deathCountBar.value + Time.deltaTime * .6f);
        }
    }

    public void checkCompletion()
    {
        checkCompletion(false);
    }

    public void checkCompletion(bool force)
    {
        StopAllCoroutines();
        if (enemiesDefeated >= currKillQuota || force)
        {
            waveMan.nextWave();
            enemiesDefeated = 0;
            currKillQuota = waveMan.GetKillQuota();
            if (FindAnyObjectByType<ProjectileConjurer>().GetProjectileEffects().Contains(ProjectileConjurer.ProjectileEffects.Regen))
            {
                FindAnyObjectByType<PlayerHealth>().PlayerAddHealth(2);
            }
            upgradeManager.SetActive(true);
            upgradeManager.GetComponent<UpgradeManager>().GetUpgrades();
        }
        else if (FindAnyObjectByType<Enemy>() == null)
        {
            StartCoroutine(Backup());
        }
    }

    private IEnumerator Backup()
    {
        yield return new WaitForSeconds(15f);
        checkCompletion(true);
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
