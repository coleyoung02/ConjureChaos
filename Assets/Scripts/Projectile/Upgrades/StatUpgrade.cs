using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgrade : Upgrade
{
    [SerializeField] protected List<Stats> statsList;
    [SerializeField] protected List<float> valuesList;
    [SerializeField] protected List<bool> modeList;
    [SerializeField] protected int healthChange;
    [SerializeField] protected bool dontHeal;
    [SerializeField] protected bool setHealthAbsolute;
    [SerializeField] protected float speedMult;
    [SerializeField] protected float invulMult;
    public override void DoUpgrade()
    {
        ProjectileConjurer conjurer = FindAnyObjectByType<ProjectileConjurer>();
        for (int i = 0; i < statsList.Count; ++i)
        {
            conjurer.UpdateStats(statsList[i], valuesList[i], modeList[i]);
        }
        if (healthChange != 0)
        {
            // need to do !dontHeal because serialized bools always default to false
            // and all of the existing ones need to be true
            FindAnyObjectByType<PlayerHealth>().ChangeMaxHealth(healthChange, setHealthAbsolute, !dontHeal);
        }
        if (speedMult != 0)
        {
            FindAnyObjectByType<PlayerMovement>().UpdateMoveSpeed(speedMult);
        }
        if (invulMult != 0)
        {
            PlayerHealth ph = FindAnyObjectByType<PlayerHealth>();
            ph.SetInvul(ph.GetInvul() * invulMult);
        }
        base.DoUpgrade();
    }
}
