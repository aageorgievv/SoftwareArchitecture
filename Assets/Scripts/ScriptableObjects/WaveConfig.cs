using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "EnemyWaves/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public List<EnemySettings> EnemyWaves;
    public float waveCooldown = 5f;
}
