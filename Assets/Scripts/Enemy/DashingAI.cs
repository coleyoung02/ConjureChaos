using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Dashing_AI : Parent_AI
{
    enum DashState
    {
        moving,
        spinning,
        dashing,
        wait,
    }
    private DashState state = DashState.moving;
    private float yOffset;
    private float xOffset;
    [SerializeField] private float xGap;
    [SerializeField] private float dashVelocity;
    [SerializeField] private float yOffsetRange;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected float dashLerpInTime;
    [SerializeField] protected float dashLerpOutTime;
    [SerializeField] protected float spinDegsPerSec;
    [SerializeField] protected float waitTime;
    private float lerpClock = 0;
    private bool lerpedIn = false;
    private Vector2 target;
    private Vector2 diff;
    private Quaternion endRot;
    private Coroutine force;
    private bool forced = false;

    // Start is called before the first frame update
    public override void Start()
    {
        SetOffsets();
        if (player != null)
        {
            player = FindFirstObjectByType<PlayerMovement>().gameObject;
        }
        if (FindObjectOfType<ProjectileConjurer>().GetProjectileEffects().Contains(ProjectileConjurer.ProjectileEffects.IAMSPEED))
        {
            dashVelocity *= 1.35f;
            spinDegsPerSec *= 1.4f;
            waitTime /= 1.5f;
        }
        base.Start();
    }
    protected void FlipSprite()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerMovement>().gameObject;
        }
        if (transform.position.x < player.transform.position.x)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    private void SetOffsets()
    {
        yOffset = UnityEngine.Random.Range(0, yOffsetRange * (2f/3f));
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerMovement>().gameObject;
        }
        if (transform.position.x > player.transform.position.x)
        {
            xOffset = xGap;
        }
        else
        {
            xOffset = -xGap;
        }
    }

    private void Moving()
    {
        FlipSprite();
        if (lerpClock > 8.5f)
        {
            state = DashState.spinning;
            lerpClock = 0f;
        }
        else
        {
            if (player.transform.position.y <= -3.5f)
            {
                yOffset = Mathf.Min(yOffsetRange / 3f, yOffset);
                yOffset = Mathf.Max(yOffset - Time.deltaTime, .5f);
            }
            Vector2 clampedDestination = (Vector2)player.transform.position + new Vector2(xOffset, yOffset);
            clampedDestination.x = Mathf.Clamp(clampedDestination.x, -13.75f, 13.75f);
            Vector2 direction = clampedDestination - (Vector2)transform.position;
            if (direction.magnitude < .75f)
            {
                state = DashState.spinning;
                lerpClock = 0f;
                rb.velocity = Vector2.zero;
            }
            else
            {
                direction.Normalize();
                rb.velocity = direction * speed;
            }
        }
    }

    private void Spinning()
    {
        Vector3 targ = player.transform.position;

        Vector3 objectPos = transform.position + Vector3.up * .1f;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = (transform.rotation.eulerAngles.z + 90f) - Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        if (player.transform.position.x > transform.position.x)
        {
            transform.Rotate(new Vector3(0, 0, -Mathf.Clamp(angle, Time.deltaTime * spinDegsPerSec, Time.deltaTime * spinDegsPerSec)));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, Mathf.Clamp(angle, Time.deltaTime * spinDegsPerSec, Time.deltaTime * spinDegsPerSec)));
        }
        if (Mathf.Abs(angle) % 360 < 2.5f || Mathf.Abs(angle) % 360 > 357.5f)
        {
            target = player.transform.position + Vector3.up * .1f;
            diff = target - (Vector2)transform.position;
            lerpClock = 0f;
            state = DashState.dashing;
        }
    }

    private void Dashing()
    {
        if (!lerpedIn)
        {
            lerpClock += Time.deltaTime;
            rb.velocity = Mathf.Lerp(0, dashVelocity, Mathf.Min(lerpClock / dashLerpInTime, 1)) * diff.normalized;
            if (lerpClock >= dashLerpInTime || Vector2.Dot((Vector2)transform.position, diff) >= 0)
            {
                endRot = transform.rotation;
                lerpedIn = true;
                lerpClock = 0f;
            }
        }
        else
        {
            if (Vector2.Dot((Vector2)transform.position, diff) >= 0 || forced)
            {
                lerpClock += Time.deltaTime;
                rb.velocity = diff.normalized * Mathf.Lerp(dashVelocity, 0f, Mathf.Min(lerpClock / dashLerpOutTime, 1));
                transform.rotation = Quaternion.Lerp(endRot, Quaternion.identity, Mathf.Min(lerpClock / dashLerpOutTime, 1));
                if (lerpClock >= dashLerpOutTime)
                {
                    state = DashState.wait;
                    lerpClock = 0f;
                    xOffset = UnityEngine.Random.Range(xGap + .5f, xGap + 3.5f);
                    yOffset = UnityEngine.Random.Range(0, yOffsetRange) + yOffsetRange / 2;
                    if (player.transform.position.x > transform.position.x)
                    {
                        xOffset *= -1;
                    }
                    lerpedIn = false;
                }
            }
            if (transform.position.y <= -4.7f)
            {
                if (rb.velocity.y > 4f)
                {
                    rb.velocity += Vector2.up * .25f * Time.deltaTime;
                    transform.position += Vector3.up * .15f * Time.deltaTime;
                    rb.velocity += Vector2.right * .25f * Time.deltaTime * Mathf.Sign(rb.velocity.x);
                }
                else
                {
                    rb.velocity += Vector2.up * .25f * Time.deltaTime * rb.velocity.y / 4f;
                    transform.position += Vector3.up * .15f * Time.deltaTime;
                    rb.velocity += Vector2.right * .25f * Time.deltaTime * Mathf.Sign(rb.velocity.x);
                }
            }
        }
    }

    private IEnumerator Force()
    {
        yield return new WaitForSeconds(.35f);
        forced = true;
    }

    private void Waiting()
    {
        FlipSprite();
        if (lerpClock > waitTime)
        {
            lerpClock = 0f;
            SetOffsets();
            state = DashState.moving;
        }
        else
        {
            lerpClock += Time.deltaTime;
            Vector2 direction = new Vector2(player.transform.position.x + xOffset, player.transform.position.y + 6) - (Vector2)transform.position;
            if (direction.magnitude < .15f)
            {
                state = DashState.moving;
            }
            else
            {
                direction.Normalize();
                rb.velocity = direction * speed;
            }
        }
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        
        if (state == DashState.moving)
        {
            Moving();
        }
        else if (state == DashState.spinning)
        {
            Spinning();
        }
        else if (state == DashState.dashing)
        {
            Dashing();
        }
        else if (state == DashState.wait)
        {
            Waiting();
        }

    }
}
