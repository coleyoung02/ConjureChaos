using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splinter : Upgrade
{
    public override void DoUpgrade()
    {
        FindObjectOfType<ShieldParent>().Activate();
        base.DoUpgrade();
    }
}

