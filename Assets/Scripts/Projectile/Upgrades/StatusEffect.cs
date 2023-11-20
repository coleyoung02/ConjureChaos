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
        Debug.Log("clicked");
        ProjectileConjurer conjurer = FindObjectOfType<ProjectileConjurer>();
        for (int i = 0; i < statusEffectsList.Count; ++i)
        {
            conjurer.UpdateStatusEffect(statusEffectsList[i], valuesList[i]);
        }
        Debug.Log("upgraded");
        base.DoUpgrade();
    }
}
