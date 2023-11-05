using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    
    private float _damage;
    private float _speed;
    private float _size;
    private float _range;

    private void Start()
    {
        InitializeStats();
        ProjectileMove();
    }

    private void InitializeStats()
    {
        ProjectileConjurer conjurer = FindObjectOfType<ProjectileConjurer>();
        Dictionary<ProjectileConjurer.Stats, float> stats = conjurer.GetStats();

        _damage = stats[ProjectileConjurer.Stats.Damage];
        _speed = stats[ProjectileConjurer.Stats.Speed];
        _size = stats[ProjectileConjurer.Stats.Size];
        _range = stats[ProjectileConjurer.Stats.Range];
    }
    
    private void ProjectileMove()
    {
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * _speed;
    }

    private void OnTriggerEnter2D(Collider2D myCollider)
    {
        if (myCollider.CompareTag("Enemy"))
        {
            // Is this the best way to do this?
            Enemy script = myCollider.gameObject.GetComponent<Enemy>();
            script.DamageEnemy(_damage);
        }
        Destroy(gameObject);
    }

}
