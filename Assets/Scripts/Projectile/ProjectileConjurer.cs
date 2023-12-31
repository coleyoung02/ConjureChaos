using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConjurer : MonoBehaviour
{
    // Fire positions
    [SerializeField]
    private float rightFirePosition;

    [SerializeField]
    AudioSource hitSFX;

    [SerializeField]
    private float leftFirePosition;
    
    // Prefab of our projectile
    [SerializeField]
    private GameObject projectilePrefab;

    // This will be set to false when its on cooldown so they can't instantly fire
    private bool _canFire = true;
    
    // Time last fired
    private float _timer = 0;
    
    // Main Camera
    private Camera _mainCamera;
    
    public enum StatusEffects
    {
        Slow,
        Fire,
    }

    public enum ProjectileEffects
    {
        EnemyPiercing,
        PlatformPiercing,
        KnockBack,
        Splinter,
        Drugs,
        Boomerang,
        IAMSPEED,
        LifeSteal,
    }

    // The float listed for fire rate is the cooldown time between shots.
    // Lowering it will give it a higher fire rate. Increasing it will give a lower fire rate.
    private Dictionary<Stats, float> _statsList = new ()
    {
        { Stats.Damage, 10f },
        { Stats.Speed, 20f},
        { Stats.Size, 0.225f},
        { Stats.Range, 10f},
        { Stats.Rate, 0.275f},
        { Stats.Accuracy, 0f }
    };

    // Keeps track of status effects that it will apply to enemy
    private Dictionary<StatusEffects, float> _statusEffects = new();
    
    // Projectile effects
    private List<ProjectileEffects> _projectileEffects = new();

    // Method so other classes can grab the stats
    public Dictionary<Stats, float> GetStats()
    {
        return _statsList;
    }
    
    // Method so other classes can grab status effects
    public Dictionary<StatusEffects, float> GetStatusEffects()
    {
        return _statusEffects;
    }

    public void PlayHitSound()
    {
        hitSFX.Play();
    }
    
    // Method so other classes can grab projectile effects
    public List<ProjectileEffects> GetProjectileEffects()
    {
        return _projectileEffects;
    }

    // Method so other classes can grab direction of projectile
    public Vector3 GetProjectileDirection()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction = Quaternion.AngleAxis(UnityEngine.Random.Range(-_statsList[Stats.Accuracy] / 2, _statsList[Stats.Accuracy]) / 2,
            new Vector3(0, 0, 1)) * direction;
        return direction;
    }

    // Method takes in a stat and changes its value
    public void UpdateStats(Stats stat, float value, bool additionMode)
    {
        if (additionMode)
        {
            _statsList[stat] += value;
        }
        else
        {
            _statsList[stat] *= value;
        }
    }
    
    // Method that takes in a status effect and duration and adds it to status effects
    public void UpdateStatusEffect(StatusEffects statusEffects, float duration)
    {
        if (_statusEffects.ContainsKey(statusEffects))
        {
            _statusEffects[statusEffects] += duration;
        }
        else
        {
            _statusEffects[statusEffects] = duration;
        }
    }
    
    // Method that takes in a projectile effect and adds it to projectile effect list
    public void UpdateProjectileEffect(ProjectileEffects projectileEffects)
    {
        _projectileEffects.Add(projectileEffects);
    }

    public void FlipFirePoint(bool flipRight)
    {
        float xPos = flipRight ? rightFirePosition : leftFirePosition;
        Transform myTransform = transform;
        Vector3 currentPos = myTransform.localPosition;
        myTransform.localPosition = new Vector3(xPos, currentPos.y, currentPos.z);
    }

    public GameObject GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        ShotCooldown();
        
        if (Input.GetMouseButton(0) && _canFire)
        {
            Transform myTransform = transform;
            Instantiate(projectilePrefab, myTransform.position, myTransform.rotation);
            _canFire = false;
        }
    }
    
    private void ShotCooldown()
    {
        if (!_canFire)
        {
            _timer += Time.deltaTime;

            if (_timer > _statsList[Stats.Rate])
            {
                _canFire = true;
                _timer = 0;
            }
        }
    }
}
