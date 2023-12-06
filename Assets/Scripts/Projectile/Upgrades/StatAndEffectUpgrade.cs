using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAndEffectUpgrade : Upgrade
{
    [SerializeField] ProjectileConjurer.ProjectileEffects effect;
    [SerializeField] protected List<Stats> statsList;
    [SerializeField] protected List<float> valuesList;
    [SerializeField] protected List<bool> modeList;
    [SerializeField] private int healthChange;
    [SerializeField] private bool setHealthAbsolute;
    [SerializeField] private float speedMult;
    
    public override void DoUpgrade()
    {
        ProjectileConjurer conjurer = FindObjectOfType<ProjectileConjurer>();
        for (int i = 0; i < statsList.Count; ++i)
        {
            conjurer.UpdateStats(statsList[i], valuesList[i], modeList[i]);
        }
        if (healthChange != 0)
        {
            FindAnyObjectByType<PlayerHealth>().ChangeMaxHealth(healthChange, setHealthAbsolute);
        }
        if (speedMult != 0)
        {
            FindAnyObjectByType<PlayerMovement>().UpdateMoveSpeed(speedMult);
        }
        FindObjectOfType<ProjectileConjurer>().UpdateProjectileEffect(effect);
        base.DoUpgrade();
    }
}
