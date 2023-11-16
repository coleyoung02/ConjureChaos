using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    List<GameObject> waves;
    Wave currWave;
    int waveNum;
    public int killQuota;

    // Start is called before the first frame update
    void Start()
    {
        waves = new List<GameObject>();
        waves.Add(Resources.Load<GameObject>("Waves/DefaultWave"));
        waveNum = 0;
        nextWave();
    }


    public void nextWave()
    {
        if(currWave != null)
            currWave.end(); //delete spawners for this wave
        waveNum++;
        if (waveNum > waves.Count)
            waveNum = 1; // temporary, for actual game this should trigger winning.
        currWave = Instantiate(waves[waveNum-1], new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Wave>();
        killQuota = currWave.getQuota();
    }
}
