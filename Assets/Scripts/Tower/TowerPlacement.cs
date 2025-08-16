using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private GameObject placementIndicatorPrefab;
    [SerializeField] private TowerButton[] towerButtons;
    private GameManager gameManager;
    private MoneyManager moneyManager;
    private TowerSelectionManager selectionManager;
    private GameObject placementIndicatorInstance;
    private TowerSlot currentHoveredSlot = null;

    private bool showIndicator = false;

    void Start()
    {
        gameManager = GameManager.GetManager<GameManager>();
        moneyManager = GameManager.GetManager<MoneyManager>();
        selectionManager = GameManager.GetManager<TowerSelectionManager>();

        foreach (TowerButton button in towerButtons)
        {
            button.OnTowerSelected += Selected;
        }
    }

    private void OnDisable()
    {
        foreach (TowerButton button in towerButtons)
        {
            button.OnTowerSelected -= Selected;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(showIndicator)
        {
            HandleHoverEffect();
        }

        if (Input.GetMouseButtonDown(0) && gameManager.CurrentGameState == GameManager.EGameState.BuildingPhase)
        {
            PlaceTower();
        }

        if(!gameManager.IsInBuildingPhase())
        {
            HidePlacementIndicator();
        }
    }

    //Places the selected tower on a valid slot if conditions are met.
    private void PlaceTower()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            TowerSlot towerSlot = hit.collider.GetComponent<TowerSlot>();
            TowerBase selectedPrefab = selectionManager.GetSelectedTowerPrefab();

            if (towerSlot != null && selectedPrefab != null && !towerSlot.isOccupied)
            {
                if (moneyManager.CanAfford(selectedPrefab.MoneyCost))
                {
                    moneyManager.SpendMoney(selectedPrefab.MoneyCost);
                    TowerBase selectedTower = Instantiate(selectedPrefab, towerSlot.transform.position, Quaternion.identity);
                    selectedTower.SetOccupiedSlot(towerSlot);
                    HidePlacementIndicator();
                    showIndicator = false;
                    selectionManager.ClearSelectedTower();
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

    //Highlights a tower slot when hovered over with the mouse.
    private void HandleHoverEffect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            TowerSlot towerSlot = hit.collider.GetComponent<TowerSlot>();

            if (towerSlot != null && towerSlot != currentHoveredSlot)
            {
                if (!towerSlot.isOccupied)
                {
                    ShowPlacementIndicator(towerSlot);
                }
            }
        }
        else
        {
            HidePlacementIndicator();
        }
    }

    //Displays a visual indicator on the hovered tower slot.
    private void ShowPlacementIndicator(TowerSlot towerSlot)
    {
        if(placementIndicatorInstance == null)
        {
            placementIndicatorInstance = Instantiate(placementIndicatorPrefab);
        }

        placementIndicatorInstance.transform.position = new Vector3(towerSlot.transform.position.x, towerSlot.transform.position.y, towerSlot.transform.position.z);
        currentHoveredSlot = towerSlot;
    }

    //Removes the placement indicator and clears the hovered slot.
    private void HidePlacementIndicator()
    {
        if(placementIndicatorInstance != null)
        {
            Destroy(placementIndicatorInstance);
            placementIndicatorInstance = null;
            currentHoveredSlot = null;
        }
    }

    //Enables the placement indicator when a tower is selected.
    private void Selected()
    {
        showIndicator = true;
    }
}
