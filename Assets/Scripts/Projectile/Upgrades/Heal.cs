using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Upgrade
{
    public override void DoUpgrade()
    {
        Debug.Log("clicked");
        FindAnyObjectByType<PlayerHealth>().HealToFull();
        Debug.Log("upgraded");
        base.DoUpgrade();
    }
}
