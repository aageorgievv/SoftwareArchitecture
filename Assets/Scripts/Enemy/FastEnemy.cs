using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : EnemyBase
{
    protected override void Start()
    {
        health = 50f;
        speed = 4f;
        money = 85;
        base.Start();
    }
}
