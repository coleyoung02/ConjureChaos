using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Flying_AI : Parent_AI
{

    // Start is called before the first frame update
     public override void Start()
     {
        base.Start();
    }
    [SerializeField] protected SpriteRenderer sprite;
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
        float dist_move = speed * Time.deltaTime;
        Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
        if (direction.magnitude < .35f)
        {
            rb.velocity = rb.velocity - rb.velocity * Time.deltaTime * 10f;
            return;
        }
        direction.Normalize();
        rb.velocity = direction * speed;
    }
}
