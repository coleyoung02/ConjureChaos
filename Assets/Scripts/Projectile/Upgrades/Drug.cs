using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drug : StatUpgrade
{
    public void Awake()
    {
        if (!FindAnyObjectByType<ProjectileConjurer>()
            .GetProjectileEffects()
            .Contains(ProjectileConjurer.ProjectileEffects.Drugs))
        {
            this.description += " +3 damage";
            addDrugs();
        }
        else
        {
            statsList = new List<Stats>();
            valuesList = new List<float>();
            modeList = new List<bool>();
        }
    }

    public override void DoUpgrade()
    {
        FindAnyObjectByType<UpgradeManager>().Drugs();
        base.DoUpgrade();
    }

    private void addDrugs()
    {
        FindAnyObjectByType<ProjectileConjurer>().UpdateProjectileEffect(ProjectileConjurer.ProjectileEffects.Drugs);
    }
}
