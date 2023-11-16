using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public GameObject spawnerPrefab;

    public int killQuota;

    public Vector2 loc1;
    public Vector2 loc2;
    public Vector2 loc3;
    public Vector2 loc4;
    public Vector2 loc5;
    public Vector2 loc6;

    public GameObject spawnType1;
    public GameObject spawnType2;
    public GameObject spawnType3;
    public GameObject spawnType4;
    public GameObject spawnType5;
    public GameObject spawnType6;

    private List<GameObject> spawners;

    // Start is called before the first frame update
    void Start()
    {
        spawners = new List<GameObject>();
        instantiateSpawners();
    }

    private void instantiateSpawners()
    {
        GameObject newSpawner;
        if(spawnType1)
        {
            newSpawner = Instantiate(spawnerPrefab);
            newSpawner.GetComponent<EnemySpawner>().enemy = spawnType1;
            newSpawner.transform.position = new Vector3(loc1.x, loc1.y, 0);
            spawners.Add(newSpawner);
        }
        if(spawnType2)
        {
            newSpawner = Instantiate(spawnerPrefab);
            newSpawner.GetComponent<EnemySpawner>().enemy = spawnType2;
            newSpawner.transform.position = new Vector3(loc2.x, loc2.y, 0);
            spawners.Add(newSpawner);
        }
        if (spawnType3)
        {
            newSpawner = Instantiate(spawnerPrefab);
            newSpawner.GetComponent<EnemySpawner>().enemy = spawnType3;
            newSpawner.transform.position = new Vector3(loc3.x, loc3.y, 0);
            spawners.Add(newSpawner);
        }
        if (spawnType4)
        {
            newSpawner = Instantiate(spawnerPrefab);
            newSpawner.GetComponent<EnemySpawner>().enemy = spawnType4;
            newSpawner.transform.position = new Vector3(loc4.x, loc4.y, 0);
            spawners.Add(newSpawner);
        }
        if (spawnType5)
        {
            newSpawner = Instantiate(spawnerPrefab);
            newSpawner.GetComponent<EnemySpawner>().enemy = spawnType5;
            newSpawner.transform.position = new Vector3(loc5.x, loc5.y, 0);
            spawners.Add(newSpawner);
        }
        if (spawnType6)
        {
            newSpawner = Instantiate(spawnerPrefab);
            newSpawner.GetComponent<EnemySpawner>().enemy = spawnType6;
            newSpawner.transform.position = new Vector3(loc6.x, loc6.y, 0);
            spawners.Add(newSpawner);
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
