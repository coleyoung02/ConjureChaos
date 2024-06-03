using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    public List<GameObject> waves;
    [SerializeField] private TextMeshProUGUI tm;
    Wave currWave;
    int waveNum;
    private int killQuota;

    // Start is called before the first frame update
    void Start()
    {
        //waves = new List<GameObject>();
        //waves.Add(Resources.Load<GameObject>("Waves/DefaultWave"));
        waveNum = 0;
        nextWave();
    }

    void Update()
    {
        if (killQuota <= 0)
        {
            killQuota = currWave.getQuota();
        }
    }

    public int GetKillQuota()
    {
        return killQuota;
    }

    private void SetWaveUI()
    {
        if (waveNum <= 15)
        {
            tm.text = "Wave " + waveNum.ToString() + "/15";
        }
        else
        {
            tm.text = "BOSS!";
        }
    }

    public void nextWave()
    {
        if(currWave != null)
            currWave.end(); //delete spawners for this wave
        waveNum++;
        SetWaveUI();
        foreach (Shooter_Projectile sp in FindObjectsByType<Shooter_Projectile>(FindObjectsSortMode.None)) {
            Destroy(sp.gameObject);
        }
        foreach (BossProjectile bp in FindObjectsByType<BossProjectile>(FindObjectsSortMode.None))
        {
            Destroy(bp.gameObject);
        }
        foreach (Projectile p in FindObjectsByType<Projectile>(FindObjectsSortMode.None))
        {
            Destroy(p.gameObject);
        }
        if (waveNum > waves.Count)
        {
            foreach (Laser l in FindObjectsByType<Laser>(FindObjectsSortMode.None))
            {
                Destroy(l.gameObject);
            }
            StartCoroutine(WaitAndWin());
            return;
        }
        currWave = Instantiate(waves[waveNum-1], gameObject.transform).GetComponent<Wave>();
        killQuota = currWave.getQuota();
    }

    private IEnumerator WaitAndWin()
    {
        yield return new WaitForSeconds(3.75f);
        SceneManager.LoadScene("WinScreen");
    }
}
