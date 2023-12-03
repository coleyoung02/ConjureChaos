using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_AI : Parent_AI
{
    public float height_thresh;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (player.transform.position.x - transform.position.x > 0)
            rb.velocity = new Vector2(speed, 0);
        else
            rb.velocity = new Vector2(-speed, 0);
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (player.transform.position.y - transform.position.y < height_thresh)
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
            direction.Normalize();
            direction *= speed;
            direction.y = rb.velocity.y;
            rb.velocity = direction;
        }
    }

    public void TurnAround(bool left)
    {
        if (player.transform.position.y - transform.position.y >= height_thresh)
        {
            if (left)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            else
                rb.velocity = new Vector2(speed, rb.velocity.y);
        }

    }
}
