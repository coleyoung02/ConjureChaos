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
    
    // The Projectile Conjurer class
    private ProjectileConjurer _conjurer;
    
    // Stats that the projectile will grab from conjuerer
    private float _damage;
    private float _speed;
    private float _size;
    private float _range;
    
    // Projectile effects
    private List<ProjectileConjurer.ProjectileEffects> _projectileEffects = new();

    // Direction for the projectile to travel
    private Vector3 _direction;
    
    // KnockBack
    private float _knockBackAmount = 5f;

    private void Start()
    {
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();

        // Initializes everything needed for projectile
        InitializeStats();
        InitializeEffects();
        InitializeSize();
        InitializeDirection();
        
        // Moves projectile
        ProjectileMove();

        Destroy(gameObject, _range / _speed);
    }

    private void InitializeStats()
    {
        Dictionary<Stats, float> stats = _conjurer.GetStats();

        _damage = stats[Stats.Damage];
        _speed = stats[Stats.Speed];
        _size = stats[Stats.Size];
        _range = stats[Stats.Range];
    }

    private void InitializeEffects()
    {
        _projectileEffects = _conjurer.GetProjectileEffects();
    }

    private void InitializeSize()
    {
        // We could just take the base size and multiply it by the size amount but for now just decided to make it the actual scale factor.
        transform.localScale = new Vector3(_size, _size, _size);
    }

    private void InitializeDirection()
    {
        _direction = _conjurer.GetProjectileDirection();
    }
    
    private void ProjectileMove()
    {
        rb.velocity = new Vector2(_direction.x, _direction.y).normalized * _speed;
    }

    private void OnTriggerEnter2D(Collider2D myCollider)
    {
        // Logic for when projectile hurts enemy
        HurtEnemy(myCollider);
        
        // Whether or not the projectile is destroyed
        DestroyingProjectileManager(myCollider);
    }

    private void HurtEnemy(Collider2D myCollider)
    {
        if (myCollider.CompareTag("Enemy"))
        {
            // Is this the best way to do this?
            Enemy script = myCollider.gameObject.GetComponent<Enemy>();
            script.DamageEnemy(_damage);
            
            // Call status effect here instead of in damage enemy function to avoid bugs
            script.StatusEffectManager();
            
            KnockBack(myCollider);
        }
    }

    private void DestroyingProjectileManager(Collider2D myCollider)
    {
        if (myCollider.CompareTag("Enemy") && _projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.EnemyPiercing))
            return;
        
        if (myCollider.CompareTag("OneWayPlatform") && _projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.PlatformPiercing))
            return;

        Destroy(gameObject);
    }

    private void KnockBack(Collider2D myCollider)
    {
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.KnockBack))
        {
            GameObject enemy = myCollider.gameObject;
            Vector3 enemyPos = enemy.transform.position;

            if (_direction.x < 0)
                _knockBackAmount *= -1f;
                
            enemy.transform.position = new Vector3(enemyPos.x + _knockBackAmount, enemyPos.y, enemyPos.z);
        }
    }
}
