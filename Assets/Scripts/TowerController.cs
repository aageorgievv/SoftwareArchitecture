using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour attackBehaviour;

    private IAttackable attackable;

    [SerializeField]
    private int attackRange = 10;
    [SerializeField]
    private int attackCooldown = 2;

    private float lastAttackTime = float.MinValue;

    void Start()
    {
        if (attackBehaviour is IAttackable)
        {
            attackable = attackBehaviour as IAttackable;
        }
        else
        {
            Debug.LogError("AttackBehaviour is not found");
        }
    }

    void Update()
    {
        Transform target = FindClosestEnemy();


        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            attackable?.Attack(target);
            lastAttackTime = Time.time;
        }
    }

    private Transform FindClosestEnemy()
    {
        //Possibly place a layer masks so it detects only enemies.
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }
}
