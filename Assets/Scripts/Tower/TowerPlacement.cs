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
                    selectionManager.SetSelectedTower();
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

    private void ShowPlacementIndicator(TowerSlot towerSlot)
    {
        if(placementIndicatorInstance == null)
        {
            placementIndicatorInstance = Instantiate(placementIndicatorPrefab);
        }

        placementIndicatorInstance.transform.position = new Vector3(towerSlot.transform.position.x, towerSlot.transform.position.y, towerSlot.transform.position.z);
        currentHoveredSlot = towerSlot;
    }

    private void HidePlacementIndicator()
    {
        if(placementIndicatorInstance != null)
        {
            Destroy(placementIndicatorInstance);
            placementIndicatorInstance = null;
            currentHoveredSlot = null;
        }
    }

    private void Selected()
    {
        showIndicator = true;
    }
}
