using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricProjectile : ProjectileBase
{
    [SerializeField]
    private float stunDuration = 1.0f;
    [SerializeField]
    private float chainAttackRange = 5f;

    [SerializeField]
    private int chainCount = 2;

    protected override void OnHitEnemy(EnemyBase enemy)
    {
        enemy.Stun(stunDuration);
        enemy.TakeDamage(damage);
        ChainToCloseEnemies(enemy.transform);
    }

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
            Debug.Log($"enemy: {i}");
        }
    }
}
