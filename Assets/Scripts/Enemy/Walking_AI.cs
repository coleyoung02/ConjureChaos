using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_AI : Parent_AI
{
    public float height_thresh;
    protected float castDistance;
    [SerializeField] protected SpriteRenderer sprite;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (player.transform.position.x - transform.position.x > 0)
            rb.linearVelocity = new Vector2(speed, 0);
        else
            rb.linearVelocity = new Vector2(-speed, 0);
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        castDistance = bc.size.y * transform.localScale.y / 2 + .5f - bc.offset.y;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (rb.linearVelocity.x > 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < .1f &&
            Mathf.Abs(player.transform.position.y - transform.position.y) < .5f)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else if (Mathf.Abs(player.transform.position.y - transform.position.y) < height_thresh ||
            (player.transform.position.y < transform.position.y - 2 
                && Mathf.Abs(player.transform.position.x) < 11f && Mathf.Abs(transform.position.x) > 10f) ||
            (rb.linearVelocity.y < -5.5f && !Physics2D.Raycast(transform.position, new Vector2(0, -1), castDistance, LayerMask.GetMask("Ground"))))
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, 0);
            direction.Normalize();
            direction *= speed;
            
            direction.y = rb.linearVelocity.y;
            rb.linearVelocity = direction;
        }
        else if (rb.linearVelocity.y < .05f)
        {
            Vector2 direction = rb.linearVelocity;
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
                if (rb.linearVelocity.x < 0)
                {
                    if (closest < 0)
                        direction.x = -speed + .65f;
                    else
                        direction.x = -speed;
                }
                else if (rb.linearVelocity.x > 0)
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
            rb.linearVelocity = direction;
        }
    }

    public void TurnAround(bool left)
    {
        if (player.transform.position.y - transform.position.y >= height_thresh)
        {
            if (left)
                rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }

    }
}
