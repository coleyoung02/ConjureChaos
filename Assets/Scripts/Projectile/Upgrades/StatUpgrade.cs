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
    public override void DoUpgrade()
    {
        Debug.Log("clicked");
        //ProjectileConjurer conjurer = FindObjectOfType<ProjectileConjurer>();
        //for (int i = 0; i < statsList.Count; ++i)
        //{
            
        //}
        //Debug.Log("upgraded");
    }
}
