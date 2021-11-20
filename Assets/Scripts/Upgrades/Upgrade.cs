using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public int cost { get; private set; }
    public int level { get; private set; }

    public Upgrade(int cost, int level)
    {

        this.cost = cost;
        this.level = level;
    }


}
