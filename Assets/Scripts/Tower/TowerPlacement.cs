using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private GameObject placementIndicatorPrefab;
    [SerializeField] private TowerButton[] towerButtons;
    private float yOffset = 0.2f;
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

        placementIndicatorInstance.transform.position = new Vector3(towerSlot.transform.position.x, towerSlot.transform.position.y - yOffset, towerSlot.transform.position.z);
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
