using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ShadowAI : Flying_Shooting_AI
{
    private Vector2 currentVelocity;
    [SerializeField] private float maxAccelerationMult;
    [SerializeField] private float maxVelocity;


    public override void Start()
    {
        base.Start();
        currentVelocity = Vector2.zero;
    }

    // Update is called once per frame

    protected override void Move()
    {
        Vector2 delta = player.transform.position - transform.position + new Vector3(6f * desiredSide, 3.5f, 0) +
            new Vector3(Mathf.Cos(Time.time) * .25f, 2f * Mathf.Sin(Time.time * 1.418f));
        // if passing close to the player, go higher
        float dX = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (dX < 4f)
        {
            delta += Vector2.up * Mathf.Lerp(5, 0, dX / 4);
        }
        if (delta.magnitude > desired_distance + .15f)
        {
            currentVelocity = delta.normalized * Mathf.Min(Mathf.Pow(delta.magnitude, 2), maxAccelerationMult);
        }
        else if (delta.magnitude < desired_distance - .15f)
        {
            currentVelocity = delta.normalized * Mathf.Min(Mathf.Pow(delta.magnitude, 2), maxAccelerationMult);
        }
        currentVelocity = currentVelocity.normalized * Mathf.Min(currentVelocity.magnitude, maxVelocity);
        rb.linearVelocity = currentVelocity;
        if (transform.position.x < -13)
        {
            desiredSide = 1;
        }
        else if (transform.position.x > 13)
        {
            desiredSide = -1;
        }
    }



    protected override void FindDesiredSide(bool defaultBehavoir=true)
    {
        if (defaultBehavoir)
        {
            base.FindDesiredSide();
        }
        else
        {
            base.FindDesiredSide();
            desiredSide *= -1;
        }
        
    }

    protected override void Shoot()
    {
        isShooting = true;
        StartCoroutine(ShootBurst(.225f, 3));
    }

    // assumes animator exists and is set up for a shooter
    private IEnumerator ShootBurst(float delayBetween, int count)
    {
        gameObject.GetComponent<Animator>().SetTrigger("Shoot");
        Debug.Log("FIRE");
        yield return new WaitForSeconds(delayBetween);
        count--;
        if (count > 0)
        {
            StartCoroutine(ShootBurst(delayBetween, count));
        }
        else
        {
            isShooting = false;
            yield return new WaitForSeconds(.25f);
            desiredSide = -desiredSide;
        }
    }
}
