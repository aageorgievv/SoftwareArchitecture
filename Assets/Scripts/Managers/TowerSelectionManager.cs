using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages tower selection for purchasing, upgrading, and selling.
/// </summary>
/// <remarks>
/// - Implements `IManager` for global access through `GameManager`.
/// - Tracks the tower prefab and projectile prefab selected for placement.
/// - Tracks an existing in-scene tower selected for upgrade or sale.
/// - Fires `OnTowerSelected` when a new tower prefab is chosen for purchase.
/// - Handles mouse clicks in the scene (while avoiding UI clicks) to select towers.
/// - Displays upgrade stats for the selected tower if it can be upgraded.
/// - Passes the selected tower to `UpgradeButtonHandler` and `SellButtonHandler` for interaction.
/// </remarks>


public class TowerSelectionManager : MonoBehaviour, IManager
{
    public event Action<TowerBase, ProjectileBase> OnTowerSelected;

    [SerializeField] private GameObject upgradeStatsPanel;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text upgradeRangeText;
    [SerializeField] private TMP_Text upgradeCooldownText;
    [SerializeField] private LayerMask placementLayer;


    private TowerBase selectedTowerPrefab;
    private ProjectileBase selectedProjectilePrefab;
    private TowerBase selectedTower;

    public void SelectTowerToBuy(TowerBase towerPrefab, ProjectileBase projectilePrefab)
    {
        selectedTowerPrefab = towerPrefab;
        selectedProjectilePrefab = projectilePrefab;
        OnTowerSelected?.Invoke(towerPrefab, projectilePrefab);
    }

    private void Start()
    {
        upgradeStatsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // Avoid UI clicks
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TowerBase tower = hit.collider.GetComponent<TowerBase>();

                if (tower != null)
                {
                    SelectTower(tower);
                } else
                {
                    upgradeStatsPanel.SetActive(false);
                }
            }
        }
    }

    private void SelectTower(TowerBase tower)
    {
        selectedTower = tower;
        Debug.Log("SelectedTower: " + selectedTower);

        if(tower.HasUpgrade)
        {
            upgradeCostText.text = $"Cost: +{tower.UpgradedCost}";
            upgradeRangeText.text = $"Range: +{tower.UpgradedRange}";
            upgradeCooldownText.text = $"Cooldown: +{tower.UpgradedCooldown}";
            upgradeStatsPanel.SetActive(true);
        }

        UpgradeButtonHandler upgradeButtonHandler = FindObjectOfType<UpgradeButtonHandler>();
        SellButtonHandler sellButtonHandler = FindObjectOfType<SellButtonHandler>();

        if (upgradeButtonHandler != null)
        {
            upgradeButtonHandler.SelectTowerForUpgrade(selectedTower);
        }

        if(sellButtonHandler != null)
        {
            sellButtonHandler.SelectTowerToSell(selectedTower);
        }
    }
    public TowerBase GetSelectedTowerPrefab()
    {
        return selectedTowerPrefab;
    }
    public TowerBase GetSelectedTowerToUpgrade()
    {
        return selectedTower;
    }

    public void SetSelectedTower()
    {
        selectedTowerPrefab = null;
    }
}
