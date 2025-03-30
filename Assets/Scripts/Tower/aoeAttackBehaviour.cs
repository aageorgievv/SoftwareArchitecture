using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttackBehaviour : MonoBehaviour, IAttackable
{
    public ProjectileBase ProjectilePrefab => projectilePrefab;

    [SerializeField]
    private ProjectileBase projectilePrefab;

    private Vector3[] directions = new Vector3[]
{
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
};

    public void Attack(Transform enemy)
    {
        ProjectileBase projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ProjectileBase projectile = projectileObject.GetComponent<ProjectileBase>();

        if (projectile != null)
        {
            foreach (Vector3 dir in directions)
            {
                projectile.Initialize(dir);
            }
        }
    }
}
