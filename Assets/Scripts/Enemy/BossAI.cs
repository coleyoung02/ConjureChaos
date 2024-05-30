using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossAI : Parent_AI
{
    private enum BossState
    {
        attacking,
        moving,
        waiting,
    }

    private enum Attack
    {
        HorizontalLasers = 0,
        HorizontalLasersVar2 = 1,
        VerticalLasers = 2,
        HomingProjectiles = 3,
        BulletHell = 4,
        EnemySpawns = 5,
    }

    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] private GameObject homingProj;
    [SerializeField] private GameObject bulletProj;
    [SerializeField] private GameObject spawnerPrefab;
    [SerializeField] private List<GameObject> validEnemies;
    [SerializeField] private int homingProjectilesCount;
    [SerializeField] private float homingProjectilesRate;
    [SerializeField] private int laserBarrageCountPerWave;
    [SerializeField] private int laserBarrageWaves;
    [SerializeField] private float laserBarrageConeSize;
    [SerializeField] private float laserBarrageRate;
    [SerializeField] private float laserBarrageWaveDelay;
    [SerializeField] private float waitTime;
    [SerializeField] private int enemiesToSpawn;
    private List<GameObject> laserWaitPoints;


    private BossState state;
    private Attack nextAttack;
    private float xOffset;
    private float yOffset;
    private int projectileCount;
    private float projectileClock;
    private float waitClock;
    private int waitIndex;
    private List<Vector2> spawnLocations;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        state = BossState.moving;
        projectileCount = 0;
        SetAttack();
        laserWaitPoints = GameObject.FindGameObjectsWithTag("LaserWaitPoint").ToList<GameObject>();
        FindAnyObjectByType<ProgressManager>().ResetForBoss();
        GameObject[] locs = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        spawnLocations = new List<Vector2>();
        foreach (GameObject go in locs)
        {
            spawnLocations.Add(go.transform.position);
        }
    }

    private void SetAttack()
    {
        int attackIndex = UnityEngine.Random.Range(0,9);
        if (attackIndex > 5)
        {
            attackIndex -= 3;
        }
        nextAttack = (Attack)attackIndex;
        //nextAttack = Attack.EnemySpawns;
        if (nextAttack == Attack.BulletHell)
        {
            yOffset = UnityEngine.Random.Range(5.45f, 6.2f);
            xOffset = UnityEngine.Random.Range(3f, 7f);
        }
        else if (nextAttack == Attack.HomingProjectiles)
        {
            yOffset = UnityEngine.Random.Range(1.25f, 3.25f);
            xOffset = UnityEngine.Random.Range(6f, 9f);
        }
        else
        {
            yOffset = UnityEngine.Random.Range(4f, 6f);
            xOffset = UnityEngine.Random.Range(4f, 7f);
        }
        xOffset *= UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    protected void FlipSprite()
    {
        if (transform.position.x < player.transform.position.x)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        FlipSprite();
        if (state == BossState.moving)
        {
            Move();
        }
        else if (state == BossState.attacking)
        {
            DoAttack();
        }
        else if (state == BossState.waiting)
        {
            if (waitClock <= 0)
            {
                SetAttack();
                state = BossState.moving;
            }
            else
            {
                if (waitClock > 2.25f)
                {
                    waitClock -= Time.deltaTime;
                    if (!(waitClock > 2.25f))
                    {
                        waitIndex = UnityEngine.Random.Range(0, laserWaitPoints.Count);
                    }
                }
                else
                {
                    waitClock -= Time.deltaTime;
                }
                Move();
            }
        }
    }
    private IEnumerator sprayRoutine()
    {
        yield return new WaitForSeconds(.2f);
        int sign = 1;
        Vector2 targetDir;
        for (int j = 0; j < laserBarrageWaves; j++)
        {
            targetDir = (Vector2)(player.transform.position - transform.position).normalized;
            if (j % 2 == 0)
                sign = 1;
            else
                sign = -1;
            for (float i = -laserBarrageConeSize / 2; i <= laserBarrageConeSize / 2 + .01f; i += laserBarrageConeSize / laserBarrageCountPerWave)
            {
                Instantiate(bulletProj, transform.position + (Vector3)targetDir * 1.5f, Quaternion.Euler(0, 0, i * sign + 180f)).GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, i * sign) * targetDir * 12f;
                yield return new WaitForSeconds(laserBarrageRate);
            }
            yield return new WaitForSeconds(laserBarrageWaveDelay);
        }
        yield return new WaitForSeconds(2f * Time.timeScale * laserBarrageWaveDelay);
    }

    private void Move()
    {
        float dist_move = speed * Time.deltaTime;
        Vector2 direction;
        if (state == BossState.waiting && (nextAttack == Attack.VerticalLasers || 
            nextAttack == Attack.HorizontalLasers || 
            nextAttack == Attack.HorizontalLasersVar2 || 
            nextAttack == Attack.EnemySpawns))
        {
            direction = (Vector2)laserWaitPoints[waitIndex].transform.position - (Vector2)transform.position;
        }
        else
        {
            direction = (Vector2)player.transform.position - (Vector2)transform.position + new Vector2(xOffset, yOffset);
        }
        if (Mathf.Abs(direction.y) < 2f)
        {
            direction.y /= 2f;
        }
        if (direction.magnitude < 7f)
        {
            rb.velocity = direction * speed / 7f;
            if (direction.magnitude < 4f && state == BossState.moving)
            {
                state = BossState.attacking;
                rb.velocity = Vector2.zero;
            }
            return;
        }
        direction.Normalize();
        rb.velocity = direction * speed;
    }

    private void DoAttack()
    {
        if (nextAttack == Attack.HorizontalLasers)
        {
            FindAnyObjectByType<BossLasers>().DoRoutine(0);
            state = BossState.waiting;
            waitClock = waitTime + 8f;
            waitIndex = UnityEngine.Random.Range(0, laserWaitPoints.Count);
        }
        else if (nextAttack == Attack.VerticalLasers)
        {
            FindAnyObjectByType<BossLasers>().VertLasers();
            state = BossState.waiting;
            waitClock = waitTime + 2.5f;
            waitIndex = UnityEngine.Random.Range(0, laserWaitPoints.Count);
        }
        else if (nextAttack == Attack.HorizontalLasersVar2)
        {
            FindAnyObjectByType<BossLasers>().DoRoutine(1);
            state = BossState.waiting;
            waitClock = waitTime + 5f;
            waitIndex = UnityEngine.Random.Range(0, laserWaitPoints.Count);
        }
        else if (nextAttack == Attack.HomingProjectiles)
        {
            Move();
            if (projectileClock <= 0)
            {
                if (projectileCount < homingProjectilesCount)
                {
                    float randDeg = UnityEngine.Random.Range(210f, 330f);
                    if (player.transform.position.x > transform.position.x)
                    {
                        randDeg -= 90f; 
                    }
                    else
                    {
                        randDeg += 90f;
                    }
                    Instantiate(homingProj, transform.position + new Vector3(Mathf.Cos(randDeg * Mathf.Deg2Rad) * 2f, Mathf.Sin(randDeg * Mathf.Deg2Rad) * 2f, 1f), Quaternion.identity);
                    projectileClock = homingProjectilesRate;
                    projectileCount += 1;
                }
                else
                {
                    projectileCount = 0;
                    projectileClock = homingProjectilesRate;
                    state = BossState.waiting;
                    waitClock = waitTime;
                }
            }
            else
            {
                projectileClock -= Time.deltaTime;
            }
        }
        else if (nextAttack == Attack.EnemySpawns)
        {
            spawnLocations = spawnLocations.OrderBy(x => Random.value).ToList();
            StartCoroutine(Spawns());
            state = BossState.waiting;
            waitClock = waitTime / 2 + enemiesToSpawn * .2f + 1.45f;
            waitIndex = UnityEngine.Random.Range(0, laserWaitPoints.Count);
        }
        else
        {
            StartCoroutine(sprayRoutine());
            state = BossState.waiting;
            waitClock = waitTime + laserBarrageWaveDelay * laserBarrageWaves + laserBarrageRate * laserBarrageCountPerWave;
        }
    }

    private IEnumerator Spawns()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            EnemySpawner tempSpawner;
            tempSpawner = Instantiate(spawnerPrefab, new Vector3(spawnLocations[i].x, spawnLocations[i].y, -4.4f), Quaternion.identity).GetComponent<EnemySpawner>();
            tempSpawner = tempSpawner.GetComponent<EnemySpawner>();
            tempSpawner.enemy = validEnemies[UnityEngine.Random.Range(0, validEnemies.Count)];
            tempSpawner.duration = .5f;
            tempSpawner.spawn_delay = 2f;
            tempSpawner.offset = 1.45f;
            yield return new WaitForSeconds(.2f);
        }
    }
}
