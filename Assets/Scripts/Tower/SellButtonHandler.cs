using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButtonHandler : MonoBehaviour
{
    [SerializeField] private Button sellButton;

    private LayerMask towerLayer;
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
