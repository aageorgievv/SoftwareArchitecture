using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionManager : MonoBehaviour, IManager
{
    private TowerBase selectedTowerPrefab;

    private ProjectileBase selectedProjectilePrefab;

    public event Action<TowerBase, ProjectileBase> OnTowerSelected;

    public void SelectTower(TowerBase towerPrefab, ProjectileBase projectilePrefab)
    {
        selectedTowerPrefab = towerPrefab;
        selectedProjectilePrefab = projectilePrefab;
        OnTowerSelected?.Invoke(towerPrefab, projectilePrefab);
    }

    public TowerBase GetSelectedTower()
    {
        return selectedTowerPrefab;
    }
}
