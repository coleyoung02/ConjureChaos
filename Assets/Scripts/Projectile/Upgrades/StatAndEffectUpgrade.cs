using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAndEffectUpgrade : StatUpgrade
{
    [SerializeField] protected ProjectileConjurer.ProjectileEffects effect;
    
    public override void DoUpgrade()
    {
        FindObjectOfType<ProjectileConjurer>().UpdateProjectileEffect(effect);
        base.DoUpgrade();
    }
}
