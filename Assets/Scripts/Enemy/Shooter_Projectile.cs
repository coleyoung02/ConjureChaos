using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Projectile : MonoBehaviour
{
    public int damage;
    [SerializeField] private bool dodgePlatoforms;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, 14f / rb.velocity.magnitude);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            FindObjectOfType<PlayerHealth>().PlayerTakeDamage(damage);
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
