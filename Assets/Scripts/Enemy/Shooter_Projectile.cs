using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Projectile : MonoBehaviour
{
    public int damage;
    [SerializeField] private bool dodgePlatoforms;
    [SerializeField] private GameObject hitEffect;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, 14f / rb.linearVelocity.magnitude);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            FindAnyObjectByType<PlayerHealth>().PlayerTakeDamage(damage);
            if (hitEffect != null)
            {
                float quatZ = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
                Instantiate(hitEffect, transform.position + (Vector3)rb.linearVelocity.normalized * .1f, Quaternion.Euler(0,0, quatZ));
            }
            Destroy(gameObject, .05f);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collider.gameObject.CompareTag("OneWayPlatform") && dodgePlatoforms)
                return;
            else
            {
                Destroy(gameObject);
            }
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            collider.gameObject.GetComponent<ShieldChild>().OnHit();
            Destroy(gameObject);
        }
    }
}
