using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_AI : Parent_AI
{
    public float height_thresh;
    protected float castDistance;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (player.transform.position.x - transform.position.x > 0)
            rb.velocity = new Vector2(speed, 0);
        else
            rb.velocity = new Vector2(-speed, 0);
        castDistance = gameObject.GetComponent<BoxCollider2D>().size.y * transform.localScale.y / 2 + .05f;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < .1f &&
            Mathf.Abs(player.transform.position.y - transform.position.y) < .5f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (Mathf.Abs(player.transform.position.y - transform.position.y) < height_thresh)
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
            direction.Normalize();
            direction *= speed;
            
            direction.y = rb.velocity.y;
            rb.velocity = direction;
        }
        else if (rb.velocity.y < .05f && 
            Physics2D.Raycast(transform.position, new Vector2(0, -1), castDistance, LayerMask.GetMask("Ground")))
        {
            Vector2 direction = rb.velocity;
            float closest = 9999f;
            Walking_AI close = null;
            foreach (Walking_AI w in FindObjectsOfType<Walking_AI>())
            {
                if (Mathf.Abs(w.gameObject.transform.position.x - gameObject.transform.position.x) < Mathf.Abs(closest) &&
                    Mathf.Abs(w.gameObject.transform.position.y - gameObject.transform.position.y) < 1 &&
                    w != this)
                {
                    closest = w.gameObject.transform.position.x - gameObject.transform.position.x;
                    close = w;
                }
            }
            if (close != null && Mathf.Abs(closest) < 3f)
            {
                if (rb.velocity.x < 0)
                {
                    if (closest < 0)
                        direction.x = -speed + .65f;
                    else
                        direction.x = -speed;
                }
                else if (rb.velocity.x > 0)
                {
                    if (closest > 0)
                        direction.x = speed - .65f;
                    else
                        direction.x = speed;
                }
            }
            else
            {
                direction = direction.normalized * speed; 
            }
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
