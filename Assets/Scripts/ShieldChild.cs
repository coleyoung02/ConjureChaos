using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ShieldChild : MonoBehaviour
{
    [SerializeField] private ShieldParent parent;
    [SerializeField] private AudioSource source;
    private Transform player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerHealth>().gameObject.transform;
    }

    private void Update()
    {
        float angle = Mathf.Atan2(transform.position.y - player.position.y, transform.position.x - player.position.x);
        source.panStereo = Mathf.Cos(angle) * .8f;
    }

    public void OnHit()
    {
        parent.OnHit();
    }

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
