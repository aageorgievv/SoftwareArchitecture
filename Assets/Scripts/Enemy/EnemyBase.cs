using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public static event Action<Vector3, int> OnEnemyDefeated;

    [SerializeField]
    protected float health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int money;
    [SerializeField]
    private int damage; // To Do

    private IMovable movable;

    [SerializeField]
    protected MoveBehaviour moveBehaviour;

    protected virtual void Start()
    {
        if (moveBehaviour != null)
        {
            movable = moveBehaviour as IMovable;
        }
        else
        {
            Debug.LogError("MoveBehaviour not found");
        }

        moveBehaviour.SetSpeed(speed);
    }

    private void OnEnable()
    {
        if (moveBehaviour is MoveBehaviour mb)
        {
            mb.OnDestinationReached += HandleDestinationReached;
        }
        else
        {
            Debug.LogError("Check event subsriptions in EnemyBase");
        }
    }

    private void OnDisable()
    {
        if (moveBehaviour is MoveBehaviour mb)
        {
            mb.OnDestinationReached -= HandleDestinationReached;
        }
        else
        {
            Debug.LogError("Check event subsriptions in EnemyBase");
        }
    }

    protected virtual void Update()
    {
        movable?.Move();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Health remaining {health}, enemy's worth {money}");

        if (health <= 0)
        {
            Die();
            OnEnemyDefeated?.Invoke(transform.position, money);
        }
    }

    protected virtual void Die()
    {
        if (EnemySpawner.Instance != null)
        {
            Destroy(gameObject);
            EnemySpawner.Instance.EnemyDefeated();
        }
        else
        {
            Debug.LogError("EnemySpawner is null");
        }
    }

    protected virtual void HandleDestinationReached(MoveBehaviour move)
    {
        if (move == moveBehaviour)
        {
            HealthManager.Instance?.ReduceLife();
            Die();
        }
    }
}
