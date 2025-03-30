using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the tower selection button in the UI, triggering the selection of a tower to buy.
/// </summary>
/// <remarks>
/// - Raises an event when a tower is bought (`OnTowerBought`).
/// - Initializes a button to select a tower when clicked.
/// - Calls the `TowerSelectionManager` to select the specified tower and its projectile.
/// </remarks>

public class TowerButton : MonoBehaviour
{
    public event Action OnTowerBought;

    [SerializeField]
    private TowerBase towerPrefab;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(SelectTower);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SelectTower);
    }

    private void SelectTower()
    {
        TowerSelectionManager towerSelectionManager = GameManager.GetManager<TowerSelectionManager>();
        towerSelectionManager.SelectTowerToBuy(towerPrefab, towerPrefab.AttackBehaviour.ProjectilePrefab);
        OnTowerBought?.Invoke();
    }
}
