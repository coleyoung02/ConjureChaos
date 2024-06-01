using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_AI : Walking_AI
{
    

    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        if (rb.velocity.x > 0)
        {
            sprite.flipX = false;
        }
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
            rb.velocity = direction;
            if (Mathf.Abs(transform.position.x - player.transform.position.x) < 3 &&
                Mathf.Abs(transform.position.x - player.transform.position.x) > 2 &&
                transform.position.y < player.transform.position.y &&
                player.transform.position.y - transform.position.y < 5f) 
            {
                Stun();
                gameObject.GetComponent<Animator>().SetTrigger("Jump");
                rb.AddForce(new Vector2(Mathf.Sign(transform.position.x - player.transform.position.x) * 2.4f, 
                    7.5f * Mathf.Pow(player.transform.position.y - transform.position.y, .25f)), ForceMode2D.Impulse);
            }
        }
    }
}
