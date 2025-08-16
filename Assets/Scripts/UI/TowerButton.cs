using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI button for selecting a tower to buy.
/// </summary>
/// <remarks>
/// - Raises `OnTowerSelected` when the button is clicked.
/// - Calls `TowerSelectionManager` to set the selected tower prefab and its projectile.
/// - Updates button interactability every frame based on the player's money and whether the game is in the building phase.
/// - Changes the button's color to indicate whether the tower can be afforded.
/// - Subscribes and unsubscribes from the button's click event on enable/disable.
/// </remarks>


public class TowerButton : MonoBehaviour
{
    public event Action OnTowerSelected;

    [SerializeField] private TowerBase towerPrefab;

    private MoneyManager moneyManager;
    private GameManager gameManager;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        gameManager = GameManager.GetManager<GameManager>();
        moneyManager = GameManager.GetManager<MoneyManager>();
    }

    private void Update()
    {
        UpdateButtonState(moneyManager.GetMoney());

        if(gameManager.IsInBuildingPhase())
        {
            button.interactable = true;
        } else
        {
            button.interactable = false;
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(SelectTower);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SelectTower);
    }

    /// <summary>
    /// Sets the selected tower in TowerSelectionManager and invokes OnTowerSelected.
    /// </summary>
    private void SelectTower()
    {
        TowerSelectionManager towerSelectionManager = GameManager.GetManager<TowerSelectionManager>();
        towerSelectionManager.SelectTowerToBuy(towerPrefab, towerPrefab.AttackBehaviour.ProjectilePrefab);
        OnTowerSelected?.Invoke();
    }

    /// <summary>
    /// Updates button interactivity and color depending on current money.
    /// </summary>
    /// <param name="currentMoney"></param>
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
