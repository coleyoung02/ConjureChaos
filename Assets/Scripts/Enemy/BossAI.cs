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
    }

    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] private GameObject homingProj;
    [SerializeField] private GameObject bulletProj;
    [SerializeField] private int homingProjectilesCount;
    [SerializeField] private float homingProjectilesRate;
    [SerializeField] private int laserBarrageCountPerWave;
    [SerializeField] private int laserBarrageWaves;
    [SerializeField] private float laserBarrageConeSize;
    [SerializeField] private float laserBarrageRate;
    [SerializeField] private float laserBarrageWaveDelay;
    [SerializeField] private float waitTime;
    private List<GameObject> laserWaitPoints;


    private BossState state;
    private Attack nextAttack;
    private float xOffset;
    private float yOffset;
    private int projectileCount;
    private float projectileClock;
    private float waitClock;
    private int waitIndex;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        state = BossState.moving;
        projectileCount = 0;
        SetAttack();
        laserWaitPoints = GameObject.FindGameObjectsWithTag("LaserWaitPoint").ToList<GameObject>();
    }

    private void SetAttack()
    {
        int attackIndex = UnityEngine.Random.Range(0,7);
        if (attackIndex > 4)
        {
            attackIndex -= 2;
        }
        nextAttack = (Attack)attackIndex;
        if (nextAttack == Attack.BulletHell)
        {
            yOffset = UnityEngine.Random.Range(4.5f, 5.8f);
            xOffset = UnityEngine.Random.Range(3f, 9f);
        }
        else if (nextAttack == Attack.HomingProjectiles)
        {
            yOffset = UnityEngine.Random.Range(1.5f, 3.5f);
            xOffset = UnityEngine.Random.Range(6f, 9f);
        }
        else
        {
            yOffset = UnityEngine.Random.Range(4f, 6f);
            xOffset = UnityEngine.Random.Range(3f, 5f);
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
                waitClock -= Time.deltaTime;
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
        if (state == BossState.waiting && (nextAttack == Attack.VerticalLasers || nextAttack == Attack.HorizontalLasers || nextAttack == Attack.HorizontalLasersVar2))
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
        else
        {
            StartCoroutine(sprayRoutine());
            state = BossState.waiting;
            waitClock = waitTime + laserBarrageWaveDelay * laserBarrageWaves + laserBarrageRate * laserBarrageCountPerWave;
        }
    }
}
