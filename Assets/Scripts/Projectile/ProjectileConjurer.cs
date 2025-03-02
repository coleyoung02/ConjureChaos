using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileConjurer : MonoBehaviour
{
    // Fire positions
    [SerializeField]
    private float rightFirePosition;

    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private List<AudioSource> sfxPool;
    [SerializeField] private float skullMultValue;
    private int sfxIndex;

    [SerializeField]
    private float leftFirePosition;
    
    // Prefab of our projectile
    [SerializeField]
    private GameObject projectilePrefab;

    // This will be set to false when its on cooldown so they can't instantly fire
    private bool _canFire = true;
    
    // Time last fired
    private float _timer = 0;
    private float _burstTimer = 0;

    // Main Camera
    private Camera _mainCamera;
    [SerializeField]
    private int maxBurstSize;
    [SerializeField]
    private Slider burstSlider;
    [SerializeField]
    private float burstRateMult;

    [SerializeField]
    private float forkingAngle;
    private int forkingCount = 0;
    private int burstAccumulated = 0;
    
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
        Homing,
        Regen,
        Blocking,
        Trail,
        SkullMult,
        BurstFire,
        Revenge,
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
        { Stats.Accuracy, 0f },
        { Stats.ShotCount, 1f },
        { Stats.SkullMult, 1f },
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
        getNextHitSource().pitch = UnityEngine.Random.Range(.9f, 1.1f);
        getNextHitSource().Play();
    }

    private AudioSource getNextHitSource()
    {
        sfxIndex = (sfxIndex + 1) % sfxPool.Count;
        return sfxPool[sfxIndex];
    }
    
    public float GetDamageScale()
    {
        return _statsList[Stats.Damage] / 10f;
    }

    public float GetSkullMult()
    {
        return _statsList[Stats.SkullMult];
    }

    public int GetNumber()
    {
        return Mathf.RoundToInt(_statsList[Stats.ShotCount]);
    }

    public float GetRateScale()
    {
        return _statsList[Stats.Rate] / 0.275f;
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
        int shots = Mathf.RoundToInt(_statsList[Stats.ShotCount]);
        if (forkingCount > 0)
        {
            if (shots % 2 == 0 && forkingCount == shots - 1)
            {
                if (UnityEngine.Random.Range(0f, 1f) > .5f)
                {
                    direction = Quaternion.AngleAxis(forkingAngle * ((forkingCount - 1) / 2 + 1), new Vector3(0, 0, 1)) * direction;
                }
                else
                {
                    direction = Quaternion.AngleAxis(-forkingAngle * ((forkingCount - 1) / 2 + 1), new Vector3(0, 0, 1)) * direction;
                }
            }
            else
            {
                direction = Quaternion.AngleAxis(((forkingCount % 2) * 2 - 1) * forkingAngle * ((forkingCount - 1) / 2 + 1), new Vector3(0, 0, 1)) * direction;
            }
        }
        forkingCount = (++forkingCount) % shots;
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
        if (projectileEffects == ProjectileEffects.LifeSteal)
        {
            FindAnyObjectByType<PlayerHealth>().ActivateLifeSteal();
        }
        if (projectileEffects == ProjectileEffects.BurstFire)
        {
            burstSlider.gameObject.SetActive(true);
        }
        if (projectileEffects == ProjectileEffects.SkullMult)
        {
            FindAnyObjectByType<PlayerHealth>().OnHealthChanged();
        }
    }

    public bool CheckHasEffect(ProjectileEffects effect)
    {
        return _projectileEffects.Contains(effect);
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
        sfxIndex = 0;
    }

    private void Update()
    {
        Shoot();
    }

    public void UpdateFromHealth(int current, int max)
    {
        if (_projectileEffects.Contains(ProjectileEffects.SkullMult))
        {
            _statsList[Stats.SkullMult] = 1 + (max - current) * skullMultValue;
        }
    }

    private void Shoot()
    {
        ShotCooldown();

        if (Input.GetMouseButton(0))
        {
            if (burstAccumulated > 0 && _burstTimer > _statsList[Stats.Rate] * burstRateMult)
            {
                Fire();
                Debug.Log("fired burst shot" + burstAccumulated);
                burstAccumulated--;
                burstSlider.value = burstAccumulated;
                _burstTimer = 0;
                if (burstAccumulated == 0)
                {
                    _timer = 0;
                    _canFire = false;
                }
            }
            else if (_canFire)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        Transform myTransform = transform;
        for (int i = 0; i < Mathf.RoundToInt(_statsList[Stats.ShotCount]); i++)
        {
            Instantiate(projectilePrefab, myTransform.position, myTransform.rotation);
        }
        _canFire = false;
    }
    
    private void ShotCooldown()
    {
        if (!_canFire && burstAccumulated <= 0)
        {
            _timer += Time.deltaTime;

            if (_timer > _statsList[Stats.Rate])
            {
                _canFire = true;
                _timer = 0;
            }
        }
        else if (_projectileEffects.Contains(ProjectileEffects.BurstFire))
        {
            if (burstAccumulated < maxBurstSize && !Input.GetMouseButton(0))
            {
                _timer += Time.deltaTime;
                if (_timer > _statsList[Stats.Rate] * 1.125f)
                {
                    burstAccumulated += 1;
                    burstSlider.value = burstAccumulated;
                    _timer = 0;
                }
            }
        }
        if (burstAccumulated > 0)
        {
            _burstTimer += Time.deltaTime;
        }
    }
}
