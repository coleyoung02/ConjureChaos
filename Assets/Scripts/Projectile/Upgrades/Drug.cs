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
            this.description += " +100% damage, +20% fire rate";
        }
        else
        {
            this.description += " You need more.";
        }
    }

    public override void DoUpgrade()
    {
        if (FindAnyObjectByType<ProjectileConjurer>()
            .GetProjectileEffects()
            .Contains(ProjectileConjurer.ProjectileEffects.Drugs))
        {
            statsList = new List<Stats>();
            valuesList = new List<float>();
            modeList = new List<bool>();
        }
        FindAnyObjectByType<UpgradeManager>().Drugs();
        addDrugs();
        base.DoUpgrade();
    }

    private void addDrugs()
    {
        FindAnyObjectByType<ProjectileConjurer>().UpdateProjectileEffect(ProjectileConjurer.ProjectileEffects.Drugs);
    }
}
