using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Upgrade
{
    public override void DoUpgrade()
    {
        FindAnyObjectByType<PlayerHealth>().HealToFull();
        FindAnyObjectByType<PlayerMovement>().UpdateMoveSpeed(1.05f);
        base.DoUpgrade();
    }
}
