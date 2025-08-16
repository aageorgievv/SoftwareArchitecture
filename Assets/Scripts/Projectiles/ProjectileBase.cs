using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for projectiles, providing common functionality for different types of projectiles.
/// </summary>
/// <remarks>
/// - Handles movement and lifetime of the projectile.
/// - Defines properties for speed, damage, and lifetime.
/// - Supports different projectile types (e.g., SingleTarget, MultiTarget).
/// - Implements collision detection to hit enemies and trigger the appropriate behavior on impact.
/// </remarks>

public abstract class ProjectileBase : MonoBehaviour
{
    public enum EProjectileType
    {
        None,
        SingleTarget,
        MultiTarget
    }

    public float Speed => speed;
    public float Damage => damage;
    public float LifeTime => lifeTime;

    public EProjectileType ProjectileType => projectileType;

    [SerializeField]
    protected float speed = 10f;
    [SerializeField]
    protected float damage = 10f;
    [SerializeField]
    protected float lifeTime = 5f;

    protected Vector3 direction;

    protected EProjectileType projectileType;

    public void Initialize(Vector3 direction)
    {
        this.direction = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    protected abstract void OnHitEnemy(EnemyBase enemy);

    /// <summary>
    /// Detects collisions with enemies, calls OnHitEnemy, and destroys the projectile.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            OnHitEnemy(enemy);
            Destroy(gameObject);
        }
    }
}
