using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitterUpgrade : Upgrade
{
    public float uploadMultiplier { get; private set; }

    public TransmitterUpgrade(
        int cost, int level, float uploadMultiplier) : base(cost, level)
    {
        this.uploadMultiplier = uploadMultiplier;
    }

}
