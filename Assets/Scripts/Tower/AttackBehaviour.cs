using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour, IAttackable
{
    public ProjectileBase ProjectilePrefab => projectilePrefab;

    [SerializeField]
    private ProjectileBase projectilePrefab;

    public void Attack(Transform enemy)
    {
        ProjectileBase projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileBase projectile = projectileObject.GetComponent<ProjectileBase>();

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
