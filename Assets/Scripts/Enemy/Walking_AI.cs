using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_AI : Parent_AI
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        float dist_move = speed * Time.deltaTime;
        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
        direction.Normalize();
        direction *= speed;
        direction.y = rb.velocity.y;
        rb.velocity = direction;
    }
}
