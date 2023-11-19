using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public GameObject spawnerPrefab;

    int killQuota;

    List<GameObject> spawnPoints;

    private List<GameObject> spawners;

    public GameObject leftFloorType;
    public float leftFloorDuration;
    public float leftFloorDelay;
    public float leftFloorOffset;

    public GameObject rightFloorType;
    public float rightFloorDuration;
    public float rightFloorDelay;
    public float rightFloorOffset;

    public GameObject leftTopType;
    public float leftTopDuration;
    public float leftTopDelay;
    public float leftTopOffset;

    public GameObject rightTopType;
    public float rightTopDuration;
    public float rightTopDelay;
    public float rightTopOffset;

    // Start is called before the first frame update
    void Start()
    {
        killQuota = 0;
        spawners = new List<GameObject>();
        spawnPoints = gameObject.transform.parent.gameObject.GetComponent<WaveManager>().spawnPoints;
        instantiateSpawners();
    }

    private void instantiateSpawners()
    {
        GameObject tempSpawner;
        EnemySpawner tempSpawnerScript;
        if(leftFloorType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[0].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = leftFloorType;
            tempSpawnerScript.duration = leftFloorDuration;
            tempSpawnerScript.spawn_delay = leftFloorDelay;
            tempSpawnerScript.offset = leftFloorOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if(rightFloorType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[1].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = rightFloorType;
            tempSpawnerScript.duration = rightFloorDuration;
            tempSpawnerScript.spawn_delay = rightFloorDelay;
            tempSpawnerScript.offset = rightFloorOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (leftTopType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[2].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = leftTopType;
            tempSpawnerScript.duration = leftTopDuration;
            tempSpawnerScript.spawn_delay = leftTopDelay;
            tempSpawnerScript.offset = leftTopOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (rightTopType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[3].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = rightTopType;
            tempSpawnerScript.duration = rightTopDuration;
            tempSpawnerScript.spawn_delay = rightTopDelay;
            tempSpawnerScript.offset = rightTopOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
    }

    public void end()
    {
        foreach (GameObject spawner in spawners)
            Destroy(spawner);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        Destroy(gameObject);
    }

    public int getQuota()
    {
        return killQuota;
    }
}
