using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Shooting_AI : Shooting_AI
{
    // Update is called once per frame
    protected override void OnUpdate()
    {
        Vector2 new_dir = new Vector2();
        //x velocity;
        if (System.Math.Abs(transform.position.x - player.transform.position.x) > desired_distance)
        {
            new_dir.x = player.transform.position.x - transform.position.x;
        }
        else if (System.Math.Abs(gameObject.transform.position.x - player.transform.position.x) < desired_distance)
        {
            if (transform.position.x - player.transform.position.x > 0)
                new_dir.x = desired_distance - (transform.position.x - player.transform.position.x);
            else
                new_dir.x = -(desired_distance + (transform.position.x - player.transform.position.x));
        }
        //y velocity
        new_dir.y = player.transform.position.y - transform.position.y;

        new_dir.Normalize();
        rb.velocity = new_dir * speed;


        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= shooting_distance)
        {
            if (cooldown == 0f)
                Shoot();
        }
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
        if (cooldown < 0f)
            cooldown = 0f;
    }
}
