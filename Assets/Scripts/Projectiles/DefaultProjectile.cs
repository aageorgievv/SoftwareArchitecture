using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : ProjectileBase
{
    protected override void Start()
    {
        projectileType = EProjectileType.SingleTarget;
        base.Start();
    }
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        enemy.TakeDamage(damage);
    }
}
