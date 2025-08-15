using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the upgrade button UI for upgrading the currently selected tower.
/// </summary>
/// <remarks>
/// - Stores the currently selected tower and enables the upgrade button only if it can be upgraded and the player can afford it.
/// - Disables the upgrade button when not in the building phase.
/// - Upgrades the selected tower when clicked, replacing it with its upgraded version and updating the selection.
/// - Interactivity is updated whenever a new tower is selected for upgrade.
/// </remarks>



public class UpgradeButtonHandler : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;
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
            sellButton.interactable = false;
        }
    }
}
