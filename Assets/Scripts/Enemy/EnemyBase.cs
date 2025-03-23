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
    protected int damage; // To Do

    [SerializeField]
    protected bool canBeStunned = true;
    [SerializeField] bool canTakeDamage = true;
    private bool isStunned = false;

    private IMovable movable;

    [SerializeField]
    protected MoveBehaviour moveBehaviour;

    private EnemySpawner enemySpawner;

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
        if (!isStunned)
        {
            movable?.Move();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        if(canTakeDamage)
        {
            health -= amount;
        }
        //Debug.Log($"Health remaining {health}, enemy's worth {money}");

        if (health <= 0)
        {
            Die();
            OnEnemyDefeated?.Invoke(transform.position, money);
        }
    }

    protected virtual void Die()
    {
        StopAllCoroutines();
        if (enemySpawner != null)
        {
            enemySpawner.EnemyDefeated();
            Destroy(gameObject);
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
            HealthManager healthManager = GameManager.GetManager<HealthManager>();
            healthManager?.ReduceLife();
            Die();
        }
    }

    public void Stun(float stunDuration)
    {
        if (!canBeStunned || isStunned)
        {
            return;
        }

        StartCoroutine(StunEffect(stunDuration));
    }

    private IEnumerator StunEffect(float duration)
    {
        isStunned = true;
        moveBehaviour.IsAgentStopped(true);
        //Debug.Log($"{gameObject.name} is stunned for {duration} seconds.");
        yield return new WaitForSeconds(duration);
        isStunned = false;
        moveBehaviour.IsAgentStopped(false);
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        enemySpawner = spawner;
    }
}
