using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectionManager : MonoBehaviour, IManager
{
    private TowerBase selectedTowerPrefab;

    public event Action<TowerBase> OnTowerSelected;

    public void SelectTower(TowerBase towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
        OnTowerSelected?.Invoke(towerPrefab);
    }

    public TowerBase GetSelectedTower()
    {
        return selectedTowerPrefab;
    }
}
