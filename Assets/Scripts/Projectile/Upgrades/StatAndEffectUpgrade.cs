using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAndEffectUpgrade : StatUpgrade
{
    [SerializeField] protected ProjectileConjurer.ProjectileEffects effect;
    
    public override void DoUpgrade()
    {
        FindAnyObjectByType<ProjectileConjurer>().UpdateProjectileEffect(effect);
        base.DoUpgrade();
    }
}
