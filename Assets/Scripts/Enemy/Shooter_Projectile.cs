using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Projectile : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collider.gameObject.GetComponent<PlayerHealth>().PlayerTakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collider.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            FindObjectOfType<ShieldParent>().OnHit();
            Destroy(gameObject);
        }
    }
}
