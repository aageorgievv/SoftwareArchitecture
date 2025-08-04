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

    private GameManager gameManager;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeSelectedTower);

        gameManager = GameManager.GetManager<GameManager>();
        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();
        upgradeButton.interactable = false;
    }

    private void Update()
    {
        if(!gameManager.IsInBuildingPhase())
        {
            upgradeButton.interactable = false;
        }
    }

    public void SelectTowerForUpgrade(TowerBase tower)
    {
        selectedTower = tower;
        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();
        upgradeButton.interactable = tower.HasUpgrade && moneyManager.GetMoney() >= selectedTower.UpgradedCost;

    }

    private void UpgradeSelectedTower()
    {
        TowerBase upgradedTower = selectedTower?.TowerUpgrade();

        if (upgradedTower != null)
        {
            Debug.Log("Tower Upgraded");
            SelectTowerForUpgrade(upgradedTower);
        }
    }
}
