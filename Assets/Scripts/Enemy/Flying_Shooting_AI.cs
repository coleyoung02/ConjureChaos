using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flying_Shooting_AI : Shooting_AI
{
    // Update is called once per frame
    protected override void OnUpdate()
    {
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
        rb.velocity = new_dir * speed;

        Debug.Log(cooldown);
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
