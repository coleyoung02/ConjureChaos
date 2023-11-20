using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : Upgrade
{
    public override void DoUpgrade()
    { 
        FindObjectOfType<ShieldParent>().Activate();
    }
}
