using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages tower selection for purchasing and upgrading in the game.
/// </summary>
/// <remarks>
/// - Implements `IManager` for global access.
/// - Tracks the selected tower and projectile prefabs for placement.
/// - Fires `OnTowerSelected` event when a new tower is chosen for purchase.
/// - Handles selecting existing towers in the scene for upgrades.
/// - Uses raycasting to detect clicked towers while avoiding UI clicks.
/// </remarks>

public class TowerSelectionManager : MonoBehaviour, IManager
{
    public event Action<TowerBase, ProjectileBase> OnTowerSelected;

    private TowerBase selectedTowerPrefab;
    private ProjectileBase selectedProjectilePrefab;
    private UpgradeTower selectedTower;

    public void SelectTowerToBuy(TowerBase towerPrefab, ProjectileBase projectilePrefab)
    {
        selectedTowerPrefab = towerPrefab;
        selectedProjectilePrefab = projectilePrefab;
        OnTowerSelected?.Invoke(towerPrefab, projectilePrefab);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // Avoid UI clicks
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                UpgradeTower tower = hit.collider.GetComponent<UpgradeTower>();
                if (tower != null)
                {
                    SelectTower(tower);
                }
            }
        }
    }

    private void SelectTower(UpgradeTower tower)
    {
        selectedTower = tower;
        Debug.Log("SelectedTower: " + selectedTower);

        UpgradeButtonHandler upgradeButtonHandler = FindObjectOfType<UpgradeButtonHandler>();
        if (upgradeButtonHandler != null)
        {
            upgradeButtonHandler.SelectTowerForUpgrade(selectedTower);
        }
    }
    public TowerBase GetSelectedTower()
    {
        return selectedTowerPrefab;
    }
    public UpgradeTower GetSelectedTowerToUpgrade()
    {
        return selectedTower;
    }
}
