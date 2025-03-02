using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class Summary
/// </summary>
public class SelectionUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text costText;
    [SerializeField]
    private TMP_Text damageText;
    [SerializeField]
    private TMP_Text rangeText;
    [SerializeField]
    private TMP_Text attackSpeedText;
    [SerializeField]
    private TMP_Text attackTypeText;

    [SerializeField]
    private GameObject panel;

    private TowerSelectionManager selectionManager;
    void Start()
    {
        panel.SetActive(false);
        selectionManager = GameManager.GetManager<TowerSelectionManager>();
        selectionManager.OnTowerSelected += UpdateUI;
    }

    private void OnDestroy()
    {
        selectionManager.OnTowerSelected -= UpdateUI;
    }

    private void UpdateUI(TowerBase tower, ProjectileBase projectile)
    {
        nameText.text = tower.name;
        costText.text = $"Cost: {tower.MoneyCost}";
        damageText.text = $"Damage: {projectile.Damage}";
        rangeText.text = $"Range: {tower.AttackRange}";
        attackSpeedText.text = $"Attack Speed: {tower.AttackCooldown}";
        attackTypeText.text = $"Attack Type: {projectile.ProjectileType}";
        panel.SetActive(true);
    }
}
