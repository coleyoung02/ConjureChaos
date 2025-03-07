using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Shooting_AI : Parent_AI
{
    public float desired_distance;
    public float shooting_distance;
    public GameObject projectilePrefab;
    public float cooldown_time;
    protected float cooldown;
    [SerializeField] private float projectile_speed;
    [SerializeField] protected SpriteRenderer sprite;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        float offset = UnityEngine.Random.Range(-.5f, .5f);
        desired_distance += offset;
        shooting_distance += offset / 2;
        cooldown = 0f;
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
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) > desired_distance)
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
            direction.Normalize();
            direction *= speed;
            direction.y = rb.linearVelocity.y;
            rb.linearVelocity = direction;
        }
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) <= shooting_distance)
        {
            if (cooldown == 0f)
                Shoot();
        }
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
        if(cooldown < 0f)
            cooldown = 0f;
    }

    protected void Shoot()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Shoot");
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        direction *= projectile_speed;
        if (FindAnyObjectByType<ProjectileConjurer>().GetProjectileEffects().Contains(ProjectileConjurer.ProjectileEffects.IAMSPEED))
            direction *= 1.85f;
        Instantiate(projectilePrefab, transform.position - Vector3.forward * .25f, Quaternion.identity).GetComponent<Rigidbody2D>().linearVelocity = direction;
        cooldown = cooldown_time;
    }
}
