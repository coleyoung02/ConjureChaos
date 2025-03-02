using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrail : MonoBehaviour
{
    public const float immunityTime = .165f;
    private ProjectileConjurer pc;
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;

    public void Start()
    {
        pc = FindAnyObjectByType<ProjectileConjurer>();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TrailDamage(damage * 
                pc.GetStats()[Stats.Damage] * pc.GetStats()[Stats.SkullMult]);
        }
    }
}
