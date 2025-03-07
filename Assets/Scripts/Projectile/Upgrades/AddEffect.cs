using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEffect : Upgrade
{
    [SerializeField] ProjectileConjurer.ProjectileEffects effect;
    public override void DoUpgrade()
    {
        FindAnyObjectByType<ProjectileConjurer>().UpdateProjectileEffect(effect);
        base.DoUpgrade();
    }
}
