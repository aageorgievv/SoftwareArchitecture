using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour moveBehaviour;

    private IMovable movable;

    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private float speed = 2;

    [SerializeField]
    private int money = 100;

    [SerializeField]
    private EnemySpawner spawner;
    [SerializeField]
    private HealthManager healthManager;

    void Start()
    {
        if (moveBehaviour != null)
        {
            movable = moveBehaviour as IMovable;
        }
        else
        {
            Debug.LogError("MoveBehaviour not found");
        }

        if(moveBehaviour is MoveBehaviour moveBeh)
        {
            moveBeh.SetSpeed(speed);
        }
    }

    void Update()
    {
        movable?.Move();
    }

    private void OnEnable()
    {
        if (moveBehaviour is MoveBehaviour mb)
        {
            mb.OnDestinationReached += HandleDestinationReached;
        }
    }

    private void OnDisable()
    {
        if (moveBehaviour is MoveBehaviour mb)
        {
            mb.OnDestinationReached -= HandleDestinationReached;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Remaining HP: {health}");

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (spawner != null)
        {
            Destroy(gameObject);
            spawner.EnemyDefeated();
        } else
        {
            Debug.LogError("EnemySpawner is null");
        }
    }

    private void HandleDestinationReached(MoveBehaviour move)
    {
        if (move == moveBehaviour)
        {
            healthManager?.ReduceLife();
            Die();
        }
    }

    public int GetCarriedMoney()
    {
        return money;
    }
}
