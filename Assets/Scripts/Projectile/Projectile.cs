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
    private GameObject particleSystemObject;

    [SerializeField]
    private GameObject nonMainProjectilePrefab;

    [SerializeField]
    private bool IsMain;

    [SerializeField]
    private GameObject hitSplat;
    [SerializeField]
    private GameObject wallHitSplat;
    [SerializeField]
    private GameObject shieldGraphic;
    [SerializeField]
    private float boomerangReverseTime = .15f;
    [SerializeField]
    private float trailPeriod;
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

    public const float burstShotDamageBoost = 1.5f;

    //change back to private
    private Collider2D ignore;

    private Vector2 initialVelocity;

    private float accumulatedTime;
    private bool flipped;
    private GameObject closestEnemy;
    private bool isBoosted = false;

    private void Awake()
    {
        boomerangReverseClock = 0f;
        // Saves the conjurer so we only have to get it once
        _conjurer = FindAnyObjectByType<ProjectileConjurer>();

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
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.Trail))
        {
            StartCoroutine(SpawnTrailParticle());
        }
    }
    public void Boost()
    {
        isBoosted = true;
        _damage *= burstShotDamageBoost;
        _speed *= 1.3f;
        ProjectileMove();
        transform.localScale *= 1.5f;
        if (particleSystemObject != null)
        {
            particleSystemObject.transform.localScale /= 1.5f;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnTrailParticle()
    {
        yield return new WaitForSeconds(trailPeriod / _speed );
        _conjurer.MakeTrail(transform, isBoosted);
        StartCoroutine(SpawnTrailParticle());
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
                    rb.linearVelocity = -rb.linearVelocity;
                    closestEnemy = GetClosestEnemy();
                    flipped = true;
                }
                rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Abs(
                    Mathf.Lerp(1, -1, 
                        Mathf.Clamp(boomerangReverseClock, 0, 2 * boomerangReverseTime) / (2 * boomerangReverseTime))) * _speed;
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
                        rb.linearVelocity = rb.linearVelocity.magnitude * transform.right;
                    }
                    else
                    {
                        gameObject.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 110f * MathF.Max(Mathf.Pow(_speed, 1.15f), 20f) / 20f));
                        rb.linearVelocity = rb.linearVelocity.magnitude * transform.right;
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
            _damage = stats[Stats.Damage] * stats[Stats.SkullMult];
        }
        else
        {
            _damage = stats[Stats.Damage] * .5f * stats[Stats.SkullMult];
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
        if (particleSystemObject != null)
        {
            particleSystemObject.transform.localScale /= 1.75f;
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
            rb.linearVelocity = new Vector2(_direction.x, _direction.y).normalized * _speed;
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (ignore != null)
        {
            rb.linearVelocity = transform.rotation * (new Vector3(_speed, 0, 0));
        }
        if (rb.linearVelocity.magnitude >= .2f)
        {
            initialVelocity = rb.linearVelocity;
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
            Vector3 hitLoc = transform.position + new Vector3(rb.linearVelocity.normalized.x, rb.linearVelocity.normalized.y) * .25f;
            Instantiate(hitSplat, hitLoc, transform.rotation, transform.parent);
            // Is this the best way to do this?
            Enemy script = myCollider.gameObject.GetComponent<Enemy>();
            hitLoc.z = -9.5f;
            hitLoc.x += UnityEngine.Random.Range(-.3f, .3f);
            hitLoc.y += UnityEngine.Random.Range(-.3f, .3f);
            script.DamageEnemy(_damage, hitLoc);

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
        bool instantiateSplat = true;
        if (myCollider.CompareTag("Enemy"))
        {
            instantiateSplat = false;
            if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.EnemyPiercing))
            {
                return;
            }
        }
        
        if (myCollider.CompareTag("OneWayPlatform") && _projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.PlatformPiercing))
            return;

        if (myCollider.CompareTag("Projectile"))
            return;

        if (myCollider.CompareTag("Laser"))
            return;

        if (instantiateSplat && IsMain)
        {
            Vector3 hitLoc = transform.position + new Vector3(rb.linearVelocity.normalized.x, rb.linearVelocity.normalized.y) * .1f;
            Instantiate(wallHitSplat, hitLoc, transform.rotation, transform.parent);
        }
        Destroy(gameObject);
    }

    private void KnockBack(Collider2D myCollider)
    {
        if (_projectileEffects.Contains(ProjectileConjurer.ProjectileEffects.KnockBack))
        {
            GameObject enemy = myCollider.gameObject;
            Vector3 enemyPos = enemy.transform.position;

            enemy.GetComponent<Parent_AI>().Stun();
            enemy.GetComponent<Rigidbody2D>().AddForce(rb.linearVelocity.normalized * _knockBackAmount, ForceMode2D.Impulse);
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
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            int numDivisions = 5;
            numDivisions += Mathf.RoundToInt(_shotCount) - 1;
            float offset = UnityEngine.Random.Range(0, 360f);
            for (int i = 180 / numDivisions; i < 360; i += 360/numDivisions)
            {
                Transform myTransform = transform;
                GameObject g = Instantiate(nonMainProjectilePrefab, 
                    myCollider.gameObject.transform.position, 
                    Quaternion.AngleAxis(angle + i + offset, Vector3.forward), 
                    transform.parent);
                Projectile p = g.GetComponent<Projectile>();
                if (isBoosted)
                {
                    p.Boost();
                }
                p.SetIgnore(myCollider);
                p.ProjectileMove();
            }
        }
    }
}
