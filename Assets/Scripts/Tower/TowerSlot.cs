using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    public bool isOccupied { get; private set; } = false;

    public void OccupySlot()
    {
        isOccupied = true;
    }

    public void FreeSlot()
    {
        isOccupied = false;
    }
}