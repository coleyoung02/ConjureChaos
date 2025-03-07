using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaosSpeed : Upgrade
{
    [SerializeField] protected List<Stats> statsList = new List<Stats>() { Stats.Rate, Stats.Speed };
    [SerializeField] protected List<float> valuesList = new List<float>(){ .55f, 1.8f };
    [SerializeField] protected List<bool> modeList = new List<bool>() { false, false };
    [SerializeField] private float speedMult = 1.5f;
    public override void DoUpgrade()
    {
        ProjectileConjurer conjurer = FindAnyObjectByType<ProjectileConjurer>();
        conjurer.UpdateProjectileEffect(ProjectileConjurer.ProjectileEffects.IAMSPEED);
        for (int i = 0; i < statsList.Count; ++i)
        {
            conjurer.UpdateStats(statsList[i], valuesList[i], modeList[i]);
        }
        FindAnyObjectByType<PlayerMovement>().UpdateMoveSpeed(speedMult);

        base.DoUpgrade();
    }
}
