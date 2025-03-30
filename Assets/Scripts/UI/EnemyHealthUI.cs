using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the display of an enemy's health on the UI.
/// </summary>
/// <remarks>
/// - Updates the health bar based on the enemy's current health.
/// - Ensures the health bar always faces the camera for visibility.
/// - Listens for health changes and updates the UI accordingly.
/// </remarks>


public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    private EnemyBase enemy;

    void Awake()
    {
        enemy = GetComponentInParent<EnemyBase>();

        if (enemy == null)
        {
            Debug.LogError("Error no enemy found!");
            return;
        }

        enemy.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = enemy.GetHealthPercentage();
        }
    }

}
