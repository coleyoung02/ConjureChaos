using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int homingProjectilesCount;
    [SerializeField] private float homingProjectilesRate;
    [SerializeField] private int laserBarrageCount;
    [SerializeField] private float laserBarrageRate;
    private List<Transform> laserWaitPoints;
    private List<Transform> bulletHellPoints;


    private BossState state;
    private Attack nextAttack;
    private float xOffset;
    private float yOffset;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        state = BossState.moving;
        SetAttack();
    }

    private void SetAttack()
    {
        int attackIndex = UnityEngine.Random.Range(0,6);
        if (attackIndex > 4)
        {
            attackIndex -= 2;
        }
        nextAttack = (Attack)attackIndex;
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

        }
    }

    private void Move()
    {
        if (nextAttack == Attack.HomingProjectiles) 
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction;
            if (transform.position.x > player.transform.position.x)
            {
                direction = (Vector2)player.transform.position - (Vector2)transform.position + new Vector2(-xOffset, yOffset);
            }
            else
            {
                direction = (Vector2)player.transform.position - (Vector2)transform.position + new Vector2(xOffset, yOffset);
            }
            if (direction.magnitude < .35f)
            {
                rb.velocity = Vector2.zero;
                state = BossState.attacking;
                return;
            }
            direction.Normalize();
            rb.velocity = direction * speed;
        }
    }

    private void DoAttack()
    {
        if (nextAttack == Attack.HorizontalLasers)
        {
            FindAnyObjectByType<BossLasers>().DoRoutine(0);
            state = BossState.waiting;
        }
        else if (nextAttack == Attack.VerticalLasers)
        {
            FindAnyObjectByType<BossLasers>().DoRoutine(1);
            state = BossState.waiting;
        }
        else if (nextAttack == Attack.VerticalLasers)
        {
            FindAnyObjectByType<BossLasers>().VertLasers();
            state = BossState.waiting;
        }
    }
}
