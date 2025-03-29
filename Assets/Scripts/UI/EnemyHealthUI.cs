using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    private EnemyBase enemy;

    void Start()
    {
        enemy = GetComponentInParent<EnemyBase>();

        if (enemy == null)
        {
            Debug.LogError("Error no enemy found!");
            return;
        }

        UpdateHealthBar();
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    private void OnEnable()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void OnDisable()
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
