using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParticles : MonoBehaviour
{
    [SerializeField] private float lifetime = .5f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
