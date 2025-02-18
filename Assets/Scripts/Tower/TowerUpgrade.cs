using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerType
{
    Normal,
    Fire,
    Ice,
    Earth
}

public abstract class TowerUpgrade : MonoBehaviour
{
    // Abstract property declaration
    public abstract ETowerType towerType { get; }

    public class BasicTower : TowerUpgrade
    {
        public override ETowerType towerType => ETowerType.Normal;
    }

    public class FireTower : TowerUpgrade
    {
        public override ETowerType towerType => ETowerType.Fire;
    }

    public class IceTower : TowerUpgrade
    {
        public override ETowerType towerType => ETowerType.Ice;
    }

    public class EarthTower : TowerUpgrade
    {
        public override ETowerType towerType => ETowerType.Earth;
    }
}
