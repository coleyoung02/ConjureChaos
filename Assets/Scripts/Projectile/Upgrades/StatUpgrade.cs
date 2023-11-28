using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgrade : Upgrade
{
    [SerializeField] protected List<Stats> statsList;
    [SerializeField] protected List<float> valuesList;
    [SerializeField] protected List<bool> modeList;
    [SerializeField] private int healthChange;
    [SerializeField] private bool setHealthAbsolute;
    public override void DoUpgrade()
    {
        Debug.Log("clicked");
        ProjectileConjurer conjurer = FindObjectOfType<ProjectileConjurer>();
        for (int i = 0; i < statsList.Count; ++i)
        {
            conjurer.UpdateStats(statsList[i], valuesList[i], modeList[i]);
        }
        if (healthChange != 0)
        {
            FindAnyObjectByType<PlayerHealth>().ChangeMaxHealth(healthChange, setHealthAbsolute);
        }

        Debug.Log("upgraded");
        base.DoUpgrade();
    }
}
