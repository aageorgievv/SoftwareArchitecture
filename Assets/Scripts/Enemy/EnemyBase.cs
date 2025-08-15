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
    public bool InstantKill { get; set; } = false;

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
        // Moves the enemy each frame if it is not stunned.
        if (!isStunned)
        {
            movable?.Move();
        }
    }

    // Reduces health by the given amount and checks if the enemy should die.
    public virtual void TakeDamage(float amount)
    {
        if (canTakeDamage)
        {
            health -= amount;
            OnHealthChanged?.Invoke();
        }

        if (health <= 0 || InstantKill)
        {
            Die();
            OnEnemyDefeated?.Invoke(transform.position, money);
        }
    }

    // Handles enemy death: rewards player, triggers events, and destroys the object.
    protected virtual void Die()
    {
        StopAllCoroutines();
        moneyManager.AddMoney(money);
        OnDeathEvent?.Invoke(this);
        Destroy(gameObject);
    }

    // Called when the enemy reaches its destination; reduces player life and kills the enemy.
    protected virtual void HandleDestinationReached(MoveBehaviour move)
    {
        if (move == moveBehaviour)
        {
            HealthManager healthManager = GameManager.GetManager<HealthManager>();
            healthManager?.ReduceLife();
            Die();
        }
    }

    // Stuns the enemy for a given duration, stopping its movement temporarily.
    public void Stun(float stunDuration)
    {
        if (!canBeStunned || isStunned)
        {
            return;
        }

        StartCoroutine(StunEffect(stunDuration));
    }

    // Coroutine that stops enemy movement for the stun duration, then resumes movement.
    private IEnumerator StunEffect(float duration)
    {
        isStunned = true;
        moveBehaviour.IsAgentStopped(true);
        yield return new WaitForSeconds(duration);
        isStunned = false;
        moveBehaviour.IsAgentStopped(false);
    }


    // Returns the enemy's current health as a percentage of its maximum health.
    public float GetHealthPercentage()
    {
        return health / maxHealth;
    }
}
