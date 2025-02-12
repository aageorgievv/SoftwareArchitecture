using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour, IAttackable
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float projectileSpeed = 10f;

    public void Attack(Transform enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector3 direction = (enemy.position - transform.position).normalized;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if(rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        } else
        {
            Debug.LogError("Projectile's RigidBody is null");
        }


    }
}
