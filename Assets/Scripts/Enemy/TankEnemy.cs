using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : EnemyBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        health = 200f;
        speed = 1f;
        money = 150;
        base.Start();
    }
}
