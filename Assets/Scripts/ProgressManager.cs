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
    [SerializeField] private GameObject upgradeManager;
    private bool boss = false;

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

    public void checkCompletion(bool force, bool bossForce=false)
    {
        if (bossForce)
        {
            StopAllCoroutines();
            waveMan.nextWave();
            return;
        }
        StopAllCoroutines();
        if (FindAnyObjectByType<PlayerHealth>().GetHealth() <= 0)
        {
            return;
        }
        // second check is if nobody has been killed in 15 seconds, 
        // and, there are no enemies remaining
        if ((!boss && enemiesDefeated >= currKillQuota) || (force && FindAnyObjectByType<Enemy>() == null))
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
            //if there are no remaining enemies, start a 15 second countdown
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

    public void SetBossProgress(float target)
    {
        boss = true;
        this.target = target;
    }

    public void ResetForBoss()
    {
        boss = true;
        deathCountBar.value = 0;
        target = 0f;
    }

    void UpdateDeathCountUI()
    {
        if (!boss)
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
}
