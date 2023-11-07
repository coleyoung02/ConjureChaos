using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct upgrade
{
    public Stats stat;
    public float value;
}
public class StatUpgrade : Upgrade
{
    [SerializeField] List<Stats> statsList;
    [SerializeField] List<float> valuesList;
    [SerializeField] List<bool> modeList;
    public override void DoUpgrade()
    {
        Debug.Log("clicked");
        ProjectileConjurer conjurer = FindObjectOfType<ProjectileConjurer>();
        for (int i = 0; i < statsList.Count; ++i)
        {
            conjurer.UpdateStats(statsList[i], valuesList[i], modeList[i]);
        }
        Debug.Log("upgraded");
    }
}
