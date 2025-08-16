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

    /// <summary>
    /// Deals damage to enemies - single target
    /// </summary>
    /// <param name="enemy"></param>
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        enemy.TakeDamage(damage);
    }
}
