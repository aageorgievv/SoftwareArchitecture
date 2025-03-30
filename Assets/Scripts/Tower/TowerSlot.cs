using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class representing a slot where a tower can be placed in the game.
/// </summary>
/// <remarks>
/// - Keeps track of whether the slot is occupied by a tower.
/// - Provides methods to occupy or free the slot.
/// </remarks>

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