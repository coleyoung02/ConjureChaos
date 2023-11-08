using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_AI : Parent_AI
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
        Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
        direction.Normalize();
        rb.velocity = direction * speed;
    }
}
