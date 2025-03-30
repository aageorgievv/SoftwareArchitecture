using UnityEngine;

/// <summary>
/// A class that defines an area-of-effect (AOE) attack behavior for towers.
/// </summary>
/// <remarks>
/// - Instantiates multiple projectiles in predefined directions (forward, back, left, right).
/// - Each projectile is initialized to move in a different direction from the tower's position.
/// - This class is intended for attacks that hit enemies in multiple directions simultaneously.
/// </remarks>


public class AoeAttackBehaviour : Attackable
{
    private Vector3[] directions = new Vector3[]
{
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
};

    public override void Attack(Transform enemy)
    {
        foreach (Vector3 dir in directions)
        {
            ProjectileBase projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileBase projectile = projectileObject.GetComponent<ProjectileBase>();
            projectile.Initialize(dir);
        }
    }
}
