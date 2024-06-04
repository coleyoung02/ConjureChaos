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

    public GameObject leftPlatType;
    public float leftPlatDuration;
    public float leftPlatDelay;
    public float leftPlatOffset;

    public GameObject rightPlatType;
    public float rightPlatDuration;
    public float rightPlatDelay;
    public float rightPlatOffset;

    public GameObject upperLeftPlatType;
    public float upperLeftPlatDuration;
    public float upperLeftPlatDelay;
    public float upperLeftPlatOffset;

    public GameObject upperRightPlatType;
    public float upperRightPlatDuration;
    public float upperRightPlatDelay;
    public float upperRightPlatOffset;

    public GameObject topType;
    public float topDuration;
    public float topDelay;
    public float topOffset;


    public GameObject centerPlatType;
    public float centerPlatDuration;
    public float centerPlatDelay;
    public float centerPlatOffset;

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
            tempSpawnerScript.debug = true;
            Debug.Log(tempSpawnerScript.NumEnemiesToSpawn());
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
        if (topType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[4].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = topType;
            tempSpawnerScript.duration = topDuration;
            tempSpawnerScript.spawn_delay = topDelay;
            tempSpawnerScript.offset = topOffset;
            tempSpawnerScript.debug = true;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (leftPlatType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[5].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = leftPlatType;
            tempSpawnerScript.duration = leftPlatDuration;
            tempSpawnerScript.spawn_delay = leftPlatDelay;
            tempSpawnerScript.offset = leftPlatOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (rightPlatType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[6].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = rightPlatType;
            tempSpawnerScript.duration = rightPlatDuration;
            tempSpawnerScript.spawn_delay = rightPlatDelay;
            tempSpawnerScript.offset = rightPlatOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (upperLeftPlatType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[7].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = upperLeftPlatType;
            tempSpawnerScript.duration = upperLeftPlatDuration;
            tempSpawnerScript.spawn_delay = upperLeftPlatDelay;
            tempSpawnerScript.offset = upperLeftPlatOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (upperRightPlatType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[8].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = upperRightPlatType;
            tempSpawnerScript.duration = upperRightPlatDuration;
            tempSpawnerScript.spawn_delay = upperRightPlatDelay;
            tempSpawnerScript.offset = upperRightPlatOffset;
            killQuota += tempSpawnerScript.NumEnemiesToSpawn();
            spawners.Add(tempSpawner);
        }
        if (centerPlatType)
        {
            tempSpawner = Instantiate(spawnerPrefab, spawnPoints[9].transform);
            tempSpawnerScript = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawnerScript.enemy = centerPlatType;
            tempSpawnerScript.duration = centerPlatDuration;
            tempSpawnerScript.spawn_delay = centerPlatDelay;
            tempSpawnerScript.offset = centerPlatOffset;
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
            if (!enemy.GetComponent<Enemy>().IsBoss())
            {
                Destroy(enemy);
            }
        }
        foreach (EnemySpawner es in FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None))
        {
            Destroy(es.gameObject);
        }
        Destroy(gameObject);
    }

    public int getQuota()
    {
        return killQuota;
    }
}
