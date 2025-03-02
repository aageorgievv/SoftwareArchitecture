using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class TowerBase : MonoBehaviour
{
    public AttackBehaviour AttackBehaviour => attackBehaviour;

    [SerializeField]
    private AttackBehaviour attackBehaviour;

    protected IAttackable attackable;

    public int AttackRange => attackRange;
    public int AttackCooldown => attackCooldown;
    public int MoneyCost => moneyCost;

    [SerializeField]
    protected int attackRange = 10;
    [SerializeField]
    protected int attackCooldown = 2;
    [SerializeField]
    protected int moneyCost;

    protected float lastAttackTime = float.MinValue;

    protected virtual void Start()
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

    protected virtual void Update()
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
