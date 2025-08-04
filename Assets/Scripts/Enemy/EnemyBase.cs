using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

/// <summary>
/// Base class for enemy behavior in a Unity tower defense game.
/// Handles movement, health, damage, stunning, and defeat mechanics.
/// </summary>
/// <remarks>
/// - Implements core enemy functionality, including taking damage, dying, and handling movement.
/// - Uses `MoveBehaviour` to control movement and subscribes to its destination-reached event.
/// - Calls `OnEnemyDefeated` when defeated, rewarding the player with money.
/// - Supports stunning with a coroutine-based effect.
/// - Interacts with `MoneyManager` and `HealthManager` to update game state.
/// </remarks>

public abstract class EnemyBase : MonoBehaviour
{
    public static event Action<Vector3, int> OnEnemyDefeated;

    public event Action OnHealthChanged;
    public event Action<EnemyBase> OnDeathEvent;

    [SerializeField] protected float maxHealth = 100f;

    [SerializeField] protected float health;
    [SerializeField] protected float speed;
    [SerializeField] protected int money;

    [SerializeField]
    protected bool canBeStunned = true;
    [SerializeField] bool canTakeDamage = true;
    private bool isStunned = false;

    private IMovable movable;

    [SerializeField]
    protected MoveBehaviour moveBehaviour;

    private MoneyManager moneyManager;

    protected virtual void Start()
    {
        health = maxHealth;

        moneyManager = GameManager.GetManager<MoneyManager>();

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
            OnHealthChanged?.Invoke();
        }

        if (health <= 0)
        {
            Die();
            OnEnemyDefeated?.Invoke(transform.position, money);
        }
    }

    protected virtual void Die()
    {
        StopAllCoroutines();
        moneyManager.AddMoney(money);
        OnDeathEvent?.Invoke(this);
        Destroy(gameObject);
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
        yield return new WaitForSeconds(duration);
        isStunned = false;
        moveBehaviour.IsAgentStopped(false);
    }

    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }
}
