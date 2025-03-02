using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public enum EProjectileType
    {
        None,
        SingleTarget,
        MultiTarget
    }
    //To do: Make this abstract class so you can have many more projectile effects
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
