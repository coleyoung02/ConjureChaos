using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flying_Shooting_AI : Shooting_AI
{
    [SerializeField] private LayerMask projectileCollisionLayers;
    // Update is called once per frame
    protected override void OnUpdate()
    {
        FlipSprite();
        Vector2 new_dir = new Vector2();
        //x velocity;
        if (System.Math.Abs(transform.position.x - player.transform.position.x) > desired_distance + .1f)
        {
            if (player.transform.position.x > transform.position.x)
                new_dir.x = player.transform.position.x - desired_distance - transform.position.x;
            else
                new_dir.x = player.transform.position.x + desired_distance - transform.position.x;
        }
        else if (System.Math.Abs(gameObject.transform.position.x - player.transform.position.x) < desired_distance - .1f)
        {
            if (transform.position.x - player.transform.position.x > 0)
                new_dir.x = desired_distance - (transform.position.x - player.transform.position.x);
            else
                new_dir.x = -(desired_distance + (transform.position.x - player.transform.position.x));
        }
        //y velocity
        new_dir.y = player.transform.position.y - transform.position.y;
        if (Mathf.Abs(new_dir.y) > .15f || Mathf.Abs(new_dir.x) > .1f)
        {
            new_dir.Normalize();
        }
        else
        {
            new_dir = Vector2.zero;
        }
        if (transform.position.x < -13.75)
        {
            new_dir.x = Mathf.Max(0, new_dir.x);
        }
        else if (transform.position.x > 13.75)
        {
            new_dir.x = Mathf.Min(0, new_dir.x);
        }
        rb.linearVelocity = new_dir * speed;

        if (!isShooting && Vector3.Distance(gameObject.transform.position, player.transform.position) <= shooting_distance)
        {
            Vector2 dir = player.transform.position - transform.position;
            RaycastHit2D rh = Physics2D.Raycast(transform.position, dir, dir.magnitude, projectileCollisionLayers);
            if (rh && rh.collider.gameObject == player.gameObject)
            {
                if (cooldown <= 0f)
                    Shoot();
            }
        }
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
    }
}
