using UnityEngine;

public abstract class Attackable : MonoBehaviour
{
    public ProjectileBase ProjectilePrefab => projectilePrefab;

    [SerializeField]
    protected ProjectileBase projectilePrefab;

    public abstract void Attack(Transform transform);
}
