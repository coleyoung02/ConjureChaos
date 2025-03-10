using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Flying_AI : Parent_AI
{
    [SerializeField] private float turnRate;
    private Vector3 lastDir;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        lastDir = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
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
        float a = Vector3.SignedAngle(lastDir, direction, Vector3.forward);
        a = Mathf.Clamp(a, -Time.deltaTime * turnRate * speed, Time.deltaTime * turnRate * speed);
        if (direction.magnitude < .35f)
        {   
            rb.linearVelocity = rb.linearVelocity - rb.linearVelocity * Time.deltaTime * 10f;
            lastDir = direction.normalized;
            return;
        }
        lastDir = Quaternion.Euler(0, 0, a) * lastDir;
        rb.linearVelocity = lastDir * speed;
    }
}
