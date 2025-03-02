using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_AI : Walking_AI
{
    [SerializeField] private float targetX;
    [SerializeField] private float deltaX;
    [SerializeField] private float jumpForceMult;
    [SerializeField] private float jumpHeight;

    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (rb.linearVelocity.x > 0)
        {
            sprite.flipX = false;
        }
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        if (Mathf.Abs(dx) < .15f &&
            (Mathf.Abs(dy) < .75f || (dy > 0 && dy < 1.5f)))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else if (Mathf.Abs(dy) < height_thresh)
        {
            float dist_move = speed * Time.deltaTime;
            Vector2 direction = new Vector2(dx, 0);
            direction.Normalize();
            direction *= speed;
            
            direction.y = rb.linearVelocity.y;
            rb.linearVelocity = direction;
        }
        else if (Mathf.Abs(rb.linearVelocity.y) < .05f &&
            Physics2D.Raycast(transform.position, new Vector2(0, -1), castDistance, LayerMask.GetMask("Ground")))
        {
            if (Mathf.Abs(dx) < targetX + deltaX &&
                Mathf.Abs(dx) > targetX - deltaX * 2 &&
                dy > 1f &&
                dy < jumpHeight) 
            {
                Stun();
                gameObject.GetComponent<Animator>().SetTrigger("Jump");
                rb.AddForce(new Vector2(Mathf.Sign(-dx) * 2.4f * Mathf.Sqrt(jumpForceMult), 
                    7.5f * Mathf.Pow(dy, .25f)) * jumpForceMult, ForceMode2D.Impulse);
            }
            else
            {
                Spacing<Jumping_AI>();
            }
        }
        if (rb.linearVelocity.magnitude < .01f)
        {
            rb.linearVelocity = new Vector2(Mathf.Sign(-dx) * speed, rb.linearVelocity.y);
        }
    }
}
