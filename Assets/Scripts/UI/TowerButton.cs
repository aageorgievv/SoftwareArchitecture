using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private TowerBase towerPrefab;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(SelectTower);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(SelectTower);
    }

    private void SelectTower()
    {
        TowerSelectionManager towerSelectionManager = GameManager.GetManager<TowerSelectionManager>();
        towerSelectionManager.SelectTower(towerPrefab, towerPrefab.AttackBehaviour.ProjectilePrefab);
    }
}
