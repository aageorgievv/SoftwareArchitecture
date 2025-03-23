using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonHandler : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    private UpgradeTower selectedTower;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeSelectedTower);

        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();

        if(moneyManager != null )
        {
            moneyManager.OnMoneyChanged += UpdateButtonState;
        }
    }

    public void SelectTowerForUpgrade(UpgradeTower tower)
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
