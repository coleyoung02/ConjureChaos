using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConjurer : MonoBehaviour
{
    // This is so we can choose an offset from where the projectile fires from the body
    [SerializeField]
    private Transform firePoint;
    
    // Prefab of our projectile
    [SerializeField]
    private GameObject projectilePrefab;

    // This will be set to false when its on cooldown so they can't instantly fire
    private bool _canFire = true;
    
    // Time last fired
    private float _timer = 0;
    
    // Main Camera
    private Camera _mainCamera;

    // The float listed for fire rate is the cooldown time between shots.
    // Lowering it will give it a higher fire rate. Increasing it will give a lower fire rate.
    private Dictionary<Stats, float> _statsList = new ()
    {
        { Stats.Damage, 10f },
        { Stats.Speed, 20f},
        { Stats.Size, 1f},
        { Stats.Range, 10f},
        { Stats.Rate, 0.3f}
    };
    
    // Method so other classes can grab the stats
    public Dictionary<Stats, float> GetStats()
    {
        return _statsList;
    }
    
    // Method so other classes can grab direction of projectile
    public Vector3 GetProjectileDirection()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
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

    private void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
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
