using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    private bool isOccupied = false;

    public void OccupySlot()
    {
        isOccupied = true;
    }

    public void FreeSlot()
    {
        isOccupied = false;
    }
}
