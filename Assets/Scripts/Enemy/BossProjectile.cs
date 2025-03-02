using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turningRate;
    [SerializeField] private GameObject poof;
    [SerializeField] private Rigidbody2D rb;
    private float currentSpeed;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
        currentSpeed = initialSpeed;
        rb.linearVelocity = (player.position - transform.position).normalized * currentSpeed;
        Vector2 diff = player.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Collider2D>() != null)
        {
            if (Vector2.Dot(player.transform.position - transform.position, transform.up) > 0)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 60f * turningRate * Mathf.Clamp(Mathf.Sqrt(currentSpeed) - 1, .75f, Mathf.Sqrt(maxSpeed - 8.5f) - 1)));
                rb.linearVelocity = currentSpeed * transform.right;
            }
            else
            {
                gameObject.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 60f * turningRate * Mathf.Clamp(Mathf.Sqrt(currentSpeed) - 1, .75f, Mathf.Sqrt(maxSpeed - 8.5f) - 1)));
                rb.linearVelocity = currentSpeed * transform.right;
            }
            currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * acceleration, maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Instantiate(poof, (Vector2)transform.position + rb.linearVelocity.normalized * .45f, Quaternion.identity);
            StopAllCoroutines();
            Destroy(rb);
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine(DamageWait());
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collision.gameObject.tag == "OneWayPlatform")
            {
                return;
            }
            Instantiate(poof, (Vector2)transform.position + rb.linearVelocity.normalized * .45f, Quaternion.identity);
            StopAllCoroutines();
            Destroy(rb);
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine(DestroyWait());
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            collision.gameObject.GetComponent<ShieldChild>().OnHit();
            Instantiate(poof, (Vector2)transform.position + rb.linearVelocity.normalized * .45f, Quaternion.identity);
            StopAllCoroutines();
            Destroy(rb);
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine(DestroyWait());
        }
    }

    private IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(.05f);
        FindAnyObjectByType<PlayerHealth>().PlayerTakeDamage(1);
        Destroy(gameObject, .05f);

    }

    private IEnumerator DestroyWait()
    {
        yield return new WaitForSeconds(.05f);
        Destroy(gameObject, .05f);
    }

}
