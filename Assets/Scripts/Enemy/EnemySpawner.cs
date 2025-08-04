using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy wave spawning and tracks active enemies.
/// </summary>
/// <remarks>
/// - Spawns enemies in waves based on `WaveConfig` data.
/// - Uses coroutines to control enemy spawning over time.
/// - Assigns travel points and behaviors to spawned enemies.
/// - Triggers `OnWaveEnd` when all enemies in a wave are defeated.
/// - Triggers `OnGameWin` when all waves are completed.
/// </remarks>

public class EnemySpawner : MonoBehaviour
{
    public event Action OnWaveEnd;
    public event Action OnGameWin;

    [SerializeField]
    private List<WaveConfig> waves;
    [SerializeField] 
    private List<Transform> travelPoints;
    [SerializeField]
    private Transform spawnPoint;

    private int currentWaveIndex = 0;

    private int activeEnemies = 0;
     
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log($"Active Enemies: {activeEnemies}");

    }

    public void StartNextWave()
    {
        if(currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        } else
        {
            Debug.Log("No Waves remaining");
        }
    }


    //Using Coroutines because spawning happens overtime and they allow controlled enemy spawning
    private IEnumerator SpawnWave(WaveConfig waveConfig)
    {
        for (int i = 0; i < waveConfig.EnemyWaves.Count; i++)
        {
            yield return StartCoroutine(spawnEnemySet(waveConfig.EnemyWaves[i]));

            // Applying cooldown after the first EnemySet, but not for the last one in the wave
            if (i < waveConfig.EnemyWaves.Count - 1)
            {
                yield return new WaitForSeconds(waveConfig.waveCooldown);
            }
        }
        currentWaveIndex++;
    }

    private IEnumerator spawnEnemySet(EnemySettings enemySet)
    {
        for(int i = 0; i < enemySet.enemyAmount; i++)
        {
            SpawnEnemy(enemySet.enemyPrefab);
            yield return new WaitForSeconds(enemySet.spawnDelay);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.SetSpawner(this);
        MoveBehaviour moveBehaviour = enemy.GetComponent<MoveBehaviour>();

        if (moveBehaviour != null)
        {
            moveBehaviour.SetTravelPoints(travelPoints);
        }
        activeEnemies++;
    }

    public void EnemyDefeated()
    {
        activeEnemies--;

        if (activeEnemies <= 0)
        {
            OnWaveEnd?.Invoke();
            Debug.Log("Wave ended");

            if (currentWaveIndex >= waves.Count)
            {
                OnGameWin?.Invoke();
                Debug.Log("Game Won!");
            }
        }
    }
}
