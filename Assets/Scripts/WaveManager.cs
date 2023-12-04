using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    public List<GameObject> waves;
    Wave currWave;
    int waveNum;
    public int killQuota;

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


    public void nextWave()
    {
        if(currWave != null)
            currWave.end(); //delete spawners for this wave
        waveNum++;
        if (waveNum > waves.Count)
            SceneManager.LoadScene("WinScreen");
        currWave = Instantiate(waves[waveNum-1], gameObject.transform).GetComponent<Wave>();
        killQuota = currWave.getQuota();
    }
}
