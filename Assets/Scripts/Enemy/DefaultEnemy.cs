using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : EnemyBase
{
    protected override void Start()
    {
        health = 100f;
        speed = 2f;
        money = 100;
        base.Start();
    }
}
