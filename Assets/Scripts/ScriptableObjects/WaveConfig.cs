using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject containing configurations for enemy waves.
/// </summary>
/// <remarks>
/// - Holds a list of `EnemySettings` that define enemy spawn details for a wave.
/// - Includes a cooldown between waves to control pacing.
/// - Can be created and configured in the Unity Editor via the "EnemyWaves/WaveConfig" menu.
/// </remarks>

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "EnemyWaves/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public List<EnemySettings> EnemyWaves;
    public float waveCooldown = 5f;
}
