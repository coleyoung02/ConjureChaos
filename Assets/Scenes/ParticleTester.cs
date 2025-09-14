using System.Collections;
using UnityEngine;

public class ParticleTester : MonoBehaviour
{
    [SerializeField] private GameObject zapEffect;
    [SerializeField] private Enemy other;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Rep());
    }

    private void DoParticles()
    {
        Vector2 delta = other.transform.position - transform.position;
        ZapEffectController zec = Instantiate(
            zapEffect,
            transform.position + Vector3.back,
            transform.rotation,
            null
            ).GetComponent<ZapEffectController>();
        zec.SetRotation(Quaternion.Euler(0, 0, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg));
        zec.SetTarget(other);
    }

    private IEnumerator Rep()
    {
        DoParticles();
        yield return new WaitForSeconds(1);
        StartCoroutine(Rep());
    }
}
