using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParticles : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, .5f);
    }
}
