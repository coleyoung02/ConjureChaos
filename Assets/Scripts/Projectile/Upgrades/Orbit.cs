using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : Upgrade
{
    public override void DoUpgrade()
    { 
        foreach (ShieldParent sp in FindObjectsOfType<ShieldParent>())
        {
            sp.Activate();
        }
        base.DoUpgrade();
    }
}
