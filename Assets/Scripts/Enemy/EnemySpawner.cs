using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float duration;
    public float time_elapsed;
    public float spawn_delay;
    public float offset;
    bool active;
    float time_until_next_spawn;
    public int spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        time_elapsed = -offset;
        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        active = true;
        time_until_next_spawn = offset;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if (time_elapsed >= duration)
                active = false;
            else if (time_until_next_spawn <= 0)            
                SpawnEnemy();

            
            time_until_next_spawn -= Time.deltaTime;
            time_elapsed += Time.deltaTime;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy_instantiated = Instantiate(enemy, transform.position, Quaternion.identity);
        enemy_instantiated.GetComponent<Parent_AI>().SetPlayer(player);
        enemy_instantiated.GetComponent<Enemy>().SetPlayer(player);
        time_until_next_spawn = spawn_delay + Time.deltaTime;
    }

    public int NumEnemiesToSpawn()
    {
        return (int)(duration / spawn_delay);
    }
}
