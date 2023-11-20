using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting_AI : Parent_AI
{
    public float desired_distance;
    public float shooting_distance;
    public GameObject projectilePrefab;
    public float cooldown_time;
    float cooldown;
    public float projectile_speed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        cooldown = 0f;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) > desired_distance)
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
            direction.Normalize();
            direction *= speed;
            direction.y = rb.velocity.y;
            rb.velocity = direction;
        }
        else
            rb.velocity = new Vector2(0, 0);
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

    void Shoot()
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        direction *= projectile_speed;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = direction;
        cooldown = cooldown_time;
    }
}
