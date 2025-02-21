using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour, IAttackable
{
    [SerializeField]
    private GameObject projectilePrefab;

    public void Attack(Transform enemy)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            Vector3 direction = (enemy.position - transform.position).normalized;
            projectile.Initialize(direction);
        }
        else
        {
            Debug.LogError("Projectile's RigidBody is null");
        }


    }
}
