using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float duration;
    public float time_elapsed;
    public float spawn_delay;
    public float offset;
    public bool debug;
    bool active;
    float time_until_next_spawn;
    public int spawnLocation;
    [SerializeField] private GameObject spawnerSprite;
    [SerializeField] private Light2D spawnerLight;
    [SerializeField] private float appearTime = .9f;
    private float timeSince;
    // Start is called before the first frame update
    void Start()
    {
        time_elapsed = -offset;
        if (!player)
            player = FindAnyObjectByType<PlayerMovement>().gameObject;
        active = true;
        time_until_next_spawn = offset;
        spawnerSprite.transform.localScale = Vector3.zero;
        timeSince = 99f;
    }

    void FixedUpdate()
    {
        if (active)
        {
            if (time_elapsed >= duration)
                active = false;
            if (time_until_next_spawn <= 0)
            {
                SpawnEnemy();
                timeSince -= spawn_delay;
            }
            else if (time_until_next_spawn <= appearTime && time_until_next_spawn < duration - time_elapsed)
            {
                spawnerSprite.transform.localScale =
                    new Vector3(spawnerSprite.transform.localScale.x + Time.deltaTime * .9f / appearTime,
                    spawnerSprite.transform.localScale.y + Time.deltaTime * .9f / appearTime,
                    spawnerSprite.transform.localScale.z + Time.deltaTime * .9f / appearTime
                    );
                spawnerLight.intensity += Time.deltaTime * 2 * .9f / appearTime;
                spawnerSprite.transform.Rotate(0, 0, Time.deltaTime * 60f);
            }
            else if (timeSince <= .3f)
            {
                spawnerSprite.transform.localScale =
                    new Vector3(spawnerSprite.transform.localScale.x - Time.deltaTime * 3 * .9f / appearTime,
                    spawnerSprite.transform.localScale.y - Time.deltaTime * 3 * .9f / appearTime,
                    spawnerSprite.transform.localScale.z - Time.deltaTime * 3 * .9f / appearTime
                    );
                spawnerLight.intensity -= Time.deltaTime * 6 * .9f / appearTime;
                spawnerSprite.transform.Rotate(0, 0, -Time.deltaTime * 180f);
            }
            else if (timeSince > .3f)
            {
                spawnerSprite.transform.localScale = Vector3.zero;
                spawnerLight.intensity = 0;
            }
            timeSince += Time.deltaTime;


            time_until_next_spawn -= Time.deltaTime;
            time_elapsed += Time.deltaTime;
        }
        else if (timeSince <= .6f)
        {
            spawnerSprite.transform.localScale =
                new Vector3(spawnerSprite.transform.localScale.x - Time.deltaTime * 3,
                spawnerSprite.transform.localScale.y - Time.deltaTime * 3,
                spawnerSprite.transform.localScale.z - Time.deltaTime * 3
                );
            spawnerLight.intensity -= Time.deltaTime * 6;
            spawnerSprite.transform.Rotate(0, 0, -Time.deltaTime * 180f);
            timeSince += Time.deltaTime;
            if (timeSince > .3f)
            {
                spawnerSprite.transform.localScale = Vector3.zero;
                spawnerLight.intensity = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy_instantiated = Instantiate(enemy, transform.position + 
            new Vector3(0, 0, -4.5f + UnityEngine.Random.Range(-.02f, .02f)), Quaternion.identity);
        enemy_instantiated.GetComponent<Parent_AI>().SetPlayer(player);
        enemy_instantiated.GetComponent<Enemy>().SetPlayer(player);
        time_until_next_spawn = spawn_delay + Time.deltaTime;
    }

    public int NumEnemiesToSpawn()
    {
        return (int)((duration - .1f) / (spawn_delay + .01f)) + 1;
    }
}
