using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProjectile : ProjectileBase
{
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        enemy.TakeDamage(damage);
    }
}
