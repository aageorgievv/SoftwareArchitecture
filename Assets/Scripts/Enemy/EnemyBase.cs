using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int money;

    protected MoveBehaviour moveBehaviour;

    protected virtual void Start()
    {
        moveBehaviour = GetComponent<MoveBehaviour>();
        moveBehaviour.SetSpeed(speed);
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Health remaining {health}, enemy's worth {money}");

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
