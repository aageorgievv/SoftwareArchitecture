using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the behavior of the upgrade button in the UI for upgrading towers.
/// </summary>
/// <remarks>
/// - Listens for button click to upgrade the selected tower.
/// - Subscribes to money changes to enable or disable the upgrade button based on available funds.
/// - Updates button interactivity based on the selected tower and player's current money.
/// </remarks>


public class UpgradeButtonHandler : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    private TowerBase selectedTower;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeSelectedTower);

        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();

        if(moneyManager != null )
        {
            moneyManager.OnMoneyChanged += UpdateButtonState;
        }
    }

    public void SelectTowerForUpgrade(TowerBase tower)
    {
        selectedTower = tower;
        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();
        UpdateButtonState(moneyManager.GetMoney());
    }

    private void UpgradeSelectedTower()
    {
        selectedTower?.TowerUpgrade();
        Debug.Log("Tower Upgraded");
    }

    private void UpdateButtonState(int currentMoney)
    {
        if(selectedTower != null)
        {
            upgradeButton.interactable = currentMoney >= selectedTower.UpgradeCost;
        } else
        {
            upgradeButton.interactable = false;
        }
    }
}
