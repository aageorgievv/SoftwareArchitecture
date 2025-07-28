using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButtonHandler : MonoBehaviour
{
    private TowerBase selectedTower;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SellTower(TowerBase tower)
    {
        selectedTower = tower;
    }
}
