using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of projectile that deals damage and stuns enemies, with the ability to chain to nearby enemies.
/// </summary>
/// <remarks>
/// - Inherits from <see cref="ProjectileBase"/> and overrides the behavior for hitting enemies.
/// - Stuns the first enemy and damages it, then chains to nearby enemies to stun and damage them.
/// - Uses the specified range and number of chain attacks for chaining to additional enemies.
/// </remarks>

public class ElectricProjectile : ProjectileBase
{
    [SerializeField]
    private float stunDuration = 1.0f;
    [SerializeField]
    private float chainAttackRange = 5f;

    [SerializeField]
    private int chainCount = 2;

    protected override void Start()
    {
        projectileType = EProjectileType.MultiTarget;
        base.Start();
    }

    //Stuns and applies damage to the struck enemy and then finds the nearest enemies and does the same to them
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        enemy.Stun(stunDuration);
        enemy.TakeDamage(damage);
        ChainToCloseEnemies(enemy.transform);
    }

    //Finds the closest enemy from the first enemy (when struck), adds them to to a list and then applies stun and damage
    private void ChainToCloseEnemies(Transform firstEnemy)
    {
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(firstEnemy.position,chainAttackRange, enemyLayer);

        List<EnemyBase> chainedEnemies = new List<EnemyBase>();

        foreach(Collider collider in colliders)
        {
            EnemyBase nextEnemy = collider.GetComponent<EnemyBase>();

            if(nextEnemy != null && nextEnemy.transform != firstEnemy)
            {
                chainedEnemies.Add(nextEnemy);
            }
        }

        for (int i = 0; i < Mathf.Min(chainCount, chainedEnemies.Count); i++)
        {
            chainedEnemies[i].Stun(stunDuration);
            chainedEnemies[i].TakeDamage(damage);
        }
    }
}
