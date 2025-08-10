using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a buildable location in the game where a tower can be placed.
/// </summary>
/// <remarks>
/// - Tracks whether the slot currently has a tower occupying it.
/// - Provides a method to update the slot's occupied status.
/// - Used by towers to mark their placement and by game logic to determine available build spots.
/// </remarks>


public class TowerSlot : MonoBehaviour
{
    public bool isOccupied { get; private set; } = false;

    public void SetIsOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }
}