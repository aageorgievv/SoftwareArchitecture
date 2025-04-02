using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class responsible for upgrading a tower in the game.
/// </summary>
/// <remarks>
/// - Manages the upgrade of a tower, including cost, range, and attack cooldown.
/// - Instantiates an upgraded version of the tower and applies the upgrade.
/// - Triggers an event when the tower is successfully upgraded.
/// </remarks>

public class UpgradeTower : MonoBehaviour
{
    public int UpgradeCost => upgradeCost;

    [SerializeField] private TowerBase upgradedTowerPrefab;
    [SerializeField] private int upgradeCost;
    [SerializeField] private int upgradeRange;
    [SerializeField] private int upgradeAttackCooldown;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void TowerUpgrade()
    {
        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();

        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not initialized.");
            return;
        }

        if (!moneyManager.CanAfford(upgradeCost))
        {
            Debug.Log("Not enough money to upgrade.");
            return;
        }

        moneyManager.SpendMoney(upgradeCost);

        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        TowerBase upgradedTower = Instantiate(upgradedTowerPrefab, currentPosition, currentRotation);
        upgradedTower.UpgradeStats(upgradeRange, upgradeAttackCooldown);
        Destroy(gameObject);
    }
}
