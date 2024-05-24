using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Collider2D>() != null)
        {
            if (Vector2.Dot(player.transform.position - transform.position, transform.up) > 0)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 60f * turningRate * Mathf.Clamp(Mathf.Sqrt(currentSpeed), 1, Mathf.Sqrt(maxSpeed) - 2)));
                rb.velocity = currentSpeed * transform.right;
            }
            else
            {
                gameObject.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 60f * turningRate * Mathf.Clamp(Mathf.Sqrt(currentSpeed), 1, Mathf.Sqrt(maxSpeed) - 2)));
                rb.velocity = currentSpeed * transform.right;
            }
            currentSpeed = Mathf.Min(currentSpeed + Time.deltaTime * acceleration, maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Instantiate(poof, (Vector2)transform.position + rb.velocity.normalized * .45f, Quaternion.identity);
            StopAllCoroutines();
            Destroy(rb);
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine(DamageWait());
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(poof, (Vector2)transform.position + rb.velocity.normalized * .45f, Quaternion.identity);
            StopAllCoroutines();
            Destroy(rb);
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine(DestroyWait());
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Debug.Log("SHIELD HIT");
            collision.gameObject.GetComponent<ShieldChild>().OnHit();
            Instantiate(poof, (Vector2)transform.position + rb.velocity.normalized * .45f, Quaternion.identity);
            StopAllCoroutines();
            Destroy(rb);
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine(DestroyWait());
        }
        else
        {
            Debug.Log("SOMETHING ELSE");
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
