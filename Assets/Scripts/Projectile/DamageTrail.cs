using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrail : MonoBehaviour
{
    public const float immunityTime = .165f;
    private ProjectileConjurer pc;
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private ParticleSystem particles;
    private float damageBoost = 1f;

    private void Awake()
    {
        pc = FindAnyObjectByType<ProjectileConjurer>();
    }

    public void OnEnable()
    {
        particles.Stop();
        StopAllCoroutines();
        particles.Play();
        StartCoroutine(DisableAfterWait());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TrailDamage(damage * damageBoost * 
                pc.GetStats()[Stats.Damage] * pc.GetStats()[Stats.SkullMult]);
        }
    }

    public void Boost(float mult)
    {
        damageBoost = mult;
    }

    public void NonBoost()
    {
        damageBoost = 1f;
    }

    private IEnumerator DisableAfterWait()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}
