using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that defines the attack behavior of towers that use projectiles.
/// </summary>
/// <remarks>
/// - Instantiates a projectile and sets its direction towards the target enemy.
/// - Assumes that the projectile has a Rigidbody and initializes it with the direction towards the enemy.
/// - Logs an error if the projectile's Rigidbody component is not found.
/// </remarks>


public class AttackBehaviour : Attackable
{
    //Instantiates a projectile, calculates direction to the target enemy, and initializes so the projectiles gets destroyed after a certain time
    public override void Attack(Transform enemy)
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
