using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines settings for enemy spawning, including prefab, amount, and spawn delay.
/// </summary>
/// <remarks>
/// - Stores information about an enemy type to be spawned in a wave.
/// - Includes a reference to the enemy prefab, the number of enemies, and the delay between spawns.
/// </remarks>

[Serializable]
public class EnemySettings
{
    public GameObject enemyPrefab;
    public int enemyAmount;
    public float spawnDelay = 1f;
}
