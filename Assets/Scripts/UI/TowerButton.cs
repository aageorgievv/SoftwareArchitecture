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
    public event Action OnTowerSelected;

    [SerializeField]
    private TowerBase towerPrefab;

    private MoneyManager moneyManager;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        moneyManager = GameManager.GetManager<MoneyManager>();
    }

    private void Update()
    {
        UpdateButtonState(moneyManager.GetMoney());
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
        OnTowerSelected?.Invoke();
    }

    private void UpdateButtonState(int currentMoney)
    {
        TowerBase tower = towerPrefab.GetComponent<TowerBase>();

        if (button != null && currentMoney >= tower.GetTowerCost())
        {
            button.interactable = true;
            button.GetComponent<Image>().color = Color.white;

        }
        else
        {
            button.interactable = false;
            button.GetComponent<Image>().color = Color.red;
        }
    }
}
