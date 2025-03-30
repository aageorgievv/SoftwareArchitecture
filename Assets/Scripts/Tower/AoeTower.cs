using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTower : TowerBase
{
    //TO DO make AOE damage
    //Maybe add some sort of effect

    public AoeAttackBehaviour AoEAttackBehaviour => AttackBehaviour as AoeAttackBehaviour;

    //protected override void Update()
    //{
    //    List<Transform> enemies = FindAllEnemiesInRange();

    //    if (enemies.Count > 0 && Time.time >= lastAttackTime + attackCooldown)
    //    {
    //        foreach (Transform enemy in enemies)
    //        {
    //            // Maybe make an AoeAttack Behaviour and place it here
    //        }
    //        lastAttackTime = Time.time;
    //    }
    //}

    //private List<Transform> FindAllEnemiesInRange()
    //{
    //    int enemyLayer = LayerMask.GetMask("Enemy");
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
    //    List<Transform> enemiesInRange = new List<Transform>();

    //    foreach (Collider collider in colliders)
    //    {
    //        if (Vector3.Distance(transform.position, collider.transform.position) <= attackRange)
    //        {
    //            enemiesInRange.Add(collider.transform);
    //        }
    //    }

    //    return enemiesInRange;
    //}
}
