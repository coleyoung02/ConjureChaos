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
    bool active;
    float time_until_next_spawn;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        time_until_next_spawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if (time_elapsed > duration)
                active = false;

            if (time_until_next_spawn <= 0)            
                SpawnEnemy();

            
            time_until_next_spawn -= Time.deltaTime;
            time_elapsed += Time.deltaTime;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy_instantiated = Instantiate(enemy, transform.position, Quaternion.identity);
        enemy_instantiated.GetComponent<Flying_AI>().player = player;
        enemy_instantiated.GetComponent<Enemy>().player = player;
        time_until_next_spawn = spawn_delay + Time.deltaTime;
    }
}
