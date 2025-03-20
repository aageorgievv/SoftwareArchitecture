using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonHandler : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    private UpgradeTower selectedTower;

    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradeSelectedTower);
    }

    public void SelectTower(UpgradeTower tower)
    {
        selectedTower = tower;
        upgradeButton.interactable = (selectedTower != null);
    }

    private void UpgradeSelectedTower()
    {
        selectedTower?.TowerUpgrade();
    }
}
