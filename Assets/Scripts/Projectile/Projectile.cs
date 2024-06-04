using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject nonMainProjectilePrefab;

    [SerializeField]
    private bool IsMain;

    [SerializeField]
    private GameObject hitSplat; 
    [SerializeField]
    private GameObject shieldGraphic;
    [SerializeField]
    private float boomerangReverseTime = .15f;
    private float boomerangReverseClock;
    [SerializeField] private GameObject damageText;

    // The Projectile Conjurer class
    private ProjectileConjurer _conjurer;
    
    // Stats that the projectile will grab from conjuerer
    private float _damage;
    private float _speed;
    private float _size;
    private float _range;
    private float _shotCount;
    private float lifetime;

    // Projectile effects
    private List<ProjectileConjurer.ProjectileEffects> _projectileEffects = new();

    // Direction for the projectile to travel
    private Vector3 _direction;
    
    // KnockBack
    private float _knockBackAmount = 25f;

    //change back to private
    public Collider2D ignore;

    private Vector2 initialVelocity;

    private float accumulatedTime;
    private bool flipped;
    private GameObject closestEnemy;

    private void Start()
    {
        boomerangReverseClock = 0f;
        // Saves the conjurer so we only have to get it once
        _conjurer = FindObjectOfType<ProjectileConjurer>();

        // Initializes everything needed for projectile
        InitializeStats();
        InitializeEffects();
        InitializeSize();
        InitializeDirection();
        
        // Moves projectile
        ProjectileMove();
        float multiplier = 1f;
        float splinterMult = 1f;
        flipped = false;
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Boomerang))
        {
            multiplier = 2.5f;
            splinterMult = 2.75f;
            if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Homing))
            {
                multiplier *= 1.25f;
                splinterMult *= 1.25f;
            }
        }
        if (IsMain)
        {
            lifetime = multiplier * _range / _speed;
            Destroy(gameObject, lifetime);
        }
        else
        {
            lifetime = splinterMult * (_range / _speed) / 2;
            Destroy(gameObject, lifetime);
        }
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Blocking))
        {
            shieldGraphic.SetActive(true);
        }
        accumulatedTime = 0f;
        boomerangReverseTime *= lifetime;
    }

    private GameObject GetClosestEnemy()
    {
        GameObject g = null;
        float closest = float.MaxValue;
        foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            if ((e.transform.position - (transform.position + transform.right * 5.5f)).magnitude < closest)
            {
                closest = (e.transform.position - transform.position).magnitude;
                g = e.gameObject;
            }
        }
        return g;
    }

    private void Update()
    {
        accumulatedTime += Time.deltaTime;
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Boomerang))
        {
            if (accumulatedTime > .3f * lifetime || (!IsMain && accumulatedTime > .15f * lifetime))
            {
                boomerangReverseClock += Time.deltaTime;

                if (boomerangReverseClock > boomerangReverseTime && !flipped)
                {
                    gameObject.transform.Rotate(new Vector3(0, 0, 180f));
                    rb.velocity = -rb.velocity;
                    closestEnemy = GetClosestEnemy();
                    flipped = true;
                }
                rb.velocity = rb.velocity.normalized * Mathf.Abs(Mathf.Lerp(1, -1, Mathf.Clamp(boomerangReverseClock, 0, 2 * boomerangReverseTime) / (2 * boomerangReverseTime))) * _speed;
            }
        }
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Homing))
        {
            if (accumulatedTime > .1f || !IsMain)
            {
                if (closestEnemy == null)
                {
                    closestEnemy = GetClosestEnemy();
                }
                else
                {
                    if (Vector2.Dot(closestEnemy.transform.position - transform.position, transform.up) > 0)
                    {
                        gameObject.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 110f * MathF.Max(Mathf.Pow(_speed, 1.15f), 20f) / 20f));
                        rb.velocity = rb.velocity.magnitude * transform.right;
                    }
                    else
                    {
                        gameObject.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 110f * MathF.Max(Mathf.Pow(_speed, 1.15f), 20f) / 20f));
                        rb.velocity = rb.velocity.magnitude * transform.right;
                    }
                }
            }
        }
    }

    private void InitializeStats()
    {
        Dictionary<Stats, float> stats = _conjurer.GetStats();

        if (IsMain)
        {
            _damage = stats[Stats.Damage];
        }
        else
        {
            _damage = stats[Stats.Damage] * .5f;
        }
        _speed = stats[Stats.Speed];
        _size = stats[Stats.Size];
        _range = stats[Stats.Range];
        _shotCount = stats[Stats.ShotCount];
    }

    private void InitializeEffects()
    {
        _projectileEffects = _conjurer.GetProjectileEffects();
    }

    private void InitializeSize()
    {
        // We could just take the base size and multiply it by the size amount but for now just decided to make it the actual scale factor.
        if (IsMain)
        {
            transform.localScale = new Vector3(_size, _size, _size);
        }
        else
        {
            transform.localScale = new Vector3(_size, _size, _size) * .7f;
        }
    }

    private void InitializeDirection()
    {
        _direction = _conjurer.GetProjectileDirection();
    }
    
    private void ProjectileMove()
    {
        if (IsMain)
        {
            rb.velocity = new Vector2(_direction.x, _direction.y).normalized * _speed;
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (ignore != null)
        {
            rb.velocity = transform.rotation * (new Vector3(_speed, 0, 0));
        }
        if (rb.velocity.magnitude >= .2f)
        {
            initialVelocity = rb.velocity;
        }

    }

    private void OnTriggerEnter2D(Collider2D myCollider)
    {
        if (myCollider != ignore || IsMain || flipped)
        {
            // Logic for when projectile hurts enemy
            HurtEnemy(myCollider);

            // Whether or not the projectile is destroyed
            DestroyingProjectileManager(myCollider);
        }
    }

    private void HurtEnemy(Collider2D myCollider)
    {
        if (myCollider.CompareTag("Enemy") && (myCollider != ignore || flipped))
        {
            Vector3 hitLoc = transform.position + new Vector3(rb.velocity.normalized.x, rb.velocity.normalized.y) * .25f;
            Instantiate(hitSplat, hitLoc, transform.rotation, null);
            // Is this the best way to do this?
            Enemy script = myCollider.gameObject.GetComponent<Enemy>();
            script.DamageEnemy(_damage);
            hitLoc.z = -9.5f;
            hitLoc.x += UnityEngine.Random.Range(-.3f, .3f);
            hitLoc.y += UnityEngine.Random.Range(-.3f, .3f);
            Instantiate(damageText, hitLoc, Quaternion.Euler(0, 0, UnityEngine.Random.Range(-7f, 7f)))
                .GetComponent<DamageNumbers>()
                .SetNumber(_damage);

            _conjurer.PlayHitSound();
            // Call status effect here instead of in damage enemy function to avoid bugs
            script.StatusEffectManager();
            
            KnockBack(myCollider);
            Splinter(myCollider);
            closestEnemy = GetClosestEnemy();
        }
    }

    private void DestroyingProjectileManager(Collider2D myCollider)
    {
        if (!IsMain && myCollider == ignore)
            return;
        
        if (myCollider.CompareTag("EnemyProjectile"))
        {
            float multiplier = .7f;
            if (IsMain)
            {
                multiplier = 1f;
            }
            float chance = .5f * multiplier * Mathf.Clamp(_conjurer.GetRateScale(), .3f, 1f) / Mathf.Sqrt(_shotCount);
            Debug.Log("chance " + chance);
            if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Blocking) && UnityEngine.Random.Range(0f, 1f) < chance)
            {
                Destroy(myCollider.transform.parent.gameObject);
                if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.EnemyPiercing))
                    return;
                Destroy(gameObject);
            }
            return;
        }

        if (myCollider.CompareTag("BossProjectile"))
        {
            float multiplier = .6f;
            if (IsMain)
            {
                multiplier = 1f;
            }
            else if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Boomerang)) 
            {
                multiplier = .4f;
            }
            float chance = .04f * multiplier * Mathf.Clamp(_conjurer.GetRateScale(), .3f, 1f) / Mathf.Sqrt(_shotCount);
            if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Blocking) && UnityEngine.Random.Range(0f, 1f) < chance)
            {
                Destroy(myCollider.transform.parent.gameObject);
                if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.EnemyPiercing))
                    return;
                Destroy(gameObject);
            }
            return;
        }

        if (myCollider.CompareTag("Enemy") && _projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.EnemyPiercing))
            return;
        
        if (myCollider.CompareTag("OneWayPlatform") && _projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.PlatformPiercing))
            return;

        if (myCollider.CompareTag("Projectile"))
            return;

        if (myCollider.CompareTag("Laser"))
            return;

        Destroy(gameObject);
    }

    private void KnockBack(Collider2D myCollider)
    {
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.KnockBack))
        {
            GameObject enemy = myCollider.gameObject;
            Vector3 enemyPos = enemy.transform.position;

            enemy.GetComponent<Parent_AI>().Stun();
            enemy.GetComponent<Rigidbody2D>().AddForce(rb.velocity.normalized * _knockBackAmount, ForceMode2D.Impulse);
        }
    }

    private void SetIgnore(Collider2D ignoreCollider)
    {
        ignore = ignoreCollider;
    }

    private void Splinter(Collider2D myCollider)
    {
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Splinter) && IsMain)
        {
            GameObject enemy = myCollider.gameObject;
            Vector3 enemyPos = enemy.transform.position;
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            int numDivisions = 5;
            numDivisions += Mathf.RoundToInt(_shotCount) - 1;
            float offset = UnityEngine.Random.Range(0, 360f);
            for (int i = 180 / numDivisions; i < 360; i += 360/numDivisions)
            {
                Transform myTransform = transform;
                GameObject g = Instantiate(nonMainProjectilePrefab, myCollider.gameObject.transform.position, Quaternion.AngleAxis(angle + i + offset, Vector3.forward));
                Projectile p = g.GetComponent<Projectile>();
                p.SetIgnore(myCollider);
                p.ProjectileMove();
            }
        }
    }
}
