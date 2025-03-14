using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusEffect : Upgrade
{
    [SerializeField] List<ProjectileConjurer.StatusEffects> statusEffectsList;
    [SerializeField] List<float> valuesList;
    public override void DoUpgrade()
    {
        ProjectileConjurer conjurer = FindAnyObjectByType<ProjectileConjurer>();
        for (int i = 0; i < statusEffectsList.Count; ++i)
        {
            conjurer.UpdateStatusEffect(statusEffectsList[i], valuesList[i]);
        }
        base.DoUpgrade();
    }
}
