using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldChild : MonoBehaviour
{
    [SerializeField] private ShieldParent parent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Is this the best way to do this?
            Enemy script = collision.gameObject.GetComponent<Enemy>();
            script.DamageEnemy(99999f);
            parent.OnHit();
        }
    }
}
