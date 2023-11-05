using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileConjurer : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;
    
    [SerializeField]
    private GameObject projectilePrefab;
    
    [SerializeField]
    private float timeBetweenFiring;

    private bool _canFire = true;
    private float _timer = 0;

    // Update is called once per frame
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

            if (_timer > timeBetweenFiring)
            {
                _canFire = true;
                _timer = 0;
            }
        }
    }
}
