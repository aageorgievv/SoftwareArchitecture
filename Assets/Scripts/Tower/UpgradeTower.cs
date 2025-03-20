using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTower : MonoBehaviour
{
    public event Action<TowerBase> OnTowerUpgraded;

    [SerializeField] private TowerBase upgradedTowerPrefab;
    [SerializeField] private int upgradeCost;

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
        OnTowerUpgraded?.Invoke(upgradedTower);
        Destroy(gameObject);
    }
}
