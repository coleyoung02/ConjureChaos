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

    // Start is called before the first frame update
    public override void Start()
    {
        SetOffsets();
        if (player != null)
        {
            player = FindFirstObjectByType<PlayerMovement>().gameObject;
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
        yOffset = UnityEngine.Random.Range(-yOffsetRange / 3f, yOffsetRange * (2f/3f));
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

    // Update is called once per frame
    protected override void OnUpdate()
    {
        FlipSprite();
        if (state == DashState.moving)
        {
            Vector2 direction = new Vector2(player.transform.position.x + xOffset, player.transform.position.y + yOffset) - (Vector2)transform.position;
            if (direction.magnitude < .15f)
            {
                state = DashState.spinning;
                rb.velocity = Vector2.zero;
            }
            else
            {
                direction.Normalize();
                rb.velocity = direction * speed;
            }
        }
        else if (state == DashState.spinning)
        {
            Vector3 targ = player.transform.position;

            Vector3 objectPos = transform.position;
            targ.x = targ.x - objectPos.x;
            targ.y = targ.y - objectPos.y;

            float angle = (transform.rotation.eulerAngles.z + 90f) - Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(angle) % 360 < 5f)
            {
                target = player.transform.position;
                diff = target - (Vector2)transform.position;
                lerpClock = 0f;
                state = DashState.dashing;
            }
            if (player.transform.position.x > transform.position.x)
            {
                transform.Rotate(new Vector3(0, 0, -Mathf.Clamp(angle, Time.deltaTime * spinDegsPerSec, Time.deltaTime * spinDegsPerSec)));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, Mathf.Clamp(angle, Time.deltaTime * spinDegsPerSec, Time.deltaTime * spinDegsPerSec)));
            }
        }
        else if (state == DashState.dashing)
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
                if (Vector2.Dot((Vector2)transform.position, diff) >= 0)
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
            }
        }
        else if (state == DashState.wait)
        {
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
                }
            }

        }
        Debug.Log(xOffset);

    }
}
