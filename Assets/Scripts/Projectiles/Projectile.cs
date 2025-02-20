using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float lifeTime = 5f;

    private Vector3 direction;

    public void Initialize(Vector3 direction)
    {
        this.direction = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        //maybe remove deltaTime
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
