using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField]
    private LayerMask placementLayer;
    private MoneyManager moneyManager;
    private TowerSelectionManager selectionManager;

    void Start()
    {
        moneyManager = GameManager.GetManager<MoneyManager>();
        selectionManager = GameManager.GetManager<TowerSelectionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceTower();
        }
    }

    private void PlaceTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            TowerSlot towerSlot = hit.collider.GetComponent<TowerSlot>();
            TowerBase selectedTower = selectionManager.GetSelectedTower();

            if (towerSlot != null && selectedTower != null && !towerSlot.isOccupied)
            {
                if (moneyManager.CanAfford(selectedTower.MoneyCost))
                {
                    moneyManager.SpendMoney(selectedTower.MoneyCost);
                    Instantiate(selectedTower, towerSlot.transform.position, Quaternion.identity);
                    towerSlot.OccupySlot();
                }
                else
                {
                    Debug.Log("You broke");
                    Debug.Log($"You have {moneyManager.GetMoney()} moneys");
                }
            }
            else
            {
                Debug.Log("Cannot Place!");
            }
        }
    }
}
