using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the sell button UI functionality for selling the currently selected tower.
/// </summary>
/// <remarks>
/// - Stores the currently selected tower to be sold.
/// - Enables or disables the sell button depending on whether a tower is selected and the game is in the building phase.
/// - Sells the selected tower when clicked, refunding its cost to the player and freeing its occupied slot.
/// - Updates the button's interactability whenever a new tower is selected or sold.
/// </remarks>

public class SellButtonHandler : MonoBehaviour
{

    [SerializeField] private Button sellButton;
    [SerializeField] private Button upgradeButton;


    private TowerBase selectedTower;
    private GameManager gameManager;
    private MoneyManager moneyManager;
    void Start()
    {
        sellButton.onClick.AddListener(SellSelectedTower);
        gameManager = GameManager.GetManager<GameManager>();
        moneyManager = GameManager.GetManager<MoneyManager>();
        sellButton.interactable = false;
    }

    private void Update()
    {
        if (!gameManager.IsInBuildingPhase())
        {
            sellButton.interactable = false;
        }
    }
    public void SelectTowerToSell(TowerBase tower)
    {
        selectedTower = tower;
        UpdateButtonState();
    }

    private void SellSelectedTower()
    {
        TowerBase tower = selectedTower;
        moneyManager.AddMoney(tower.MoneyCost);
        tower.SetOccupiedSlot(null);
        Destroy(tower.gameObject);
        UpdateButtonState();
        upgradeButton.interactable = false;
        sellButton.interactable = false;

    }

    private void UpdateButtonState()
    {
        if (selectedTower != null)
        {
            sellButton.interactable = true;
        }
        else
        {
            sellButton.interactable = false;
        }
    }
}
