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
        if (EnemySpawner.Instance != null)
        {
            Destroy(gameObject);
            EnemySpawner.Instance.EnemyDefeated();
        } else
        {
            Debug.LogError("EnemySpawner is null");
        }
    }

    private void HandleDestinationReached(MoveBehaviour move)
    {
        if (move == moveBehaviour)
        {
            Die();
        }
    }
}
