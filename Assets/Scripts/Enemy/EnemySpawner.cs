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

    private readonly List<EnemyBase> spawnedEnemies = new List<EnemyBase>();

    private int currentWaveIndex = 0;
    private bool isSpawning;

    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log($"Active Enemies: {spawnedEnemies.Count}");
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
        isSpawning = true;
        int currentWave = currentWaveIndex;
        Debug.Log($"Spawn Wave {currentWave}");

        for (int i = 0; i < waveConfig.EnemyWaves.Count; i++)
        {
            yield return StartCoroutine(SpawnEnemySet(waveConfig.EnemyWaves[i]));

            // Applying cooldown after the first EnemySet, but not for the last one in the wave
            if (i < waveConfig.EnemyWaves.Count - 1)
            {
                Debug.Log($"Spawned Set ({i + 1}/{waveConfig.EnemyWaves.Count}) for Wave {currentWave}");
                yield return new WaitForSeconds(waveConfig.waveCooldown);
            }
        }
        currentWaveIndex++;
        isSpawning = false;
    }

    private IEnumerator SpawnEnemySet(EnemySettings enemySet)
    {
        Debug.Log($"Spawn Enemy Set");

        for (int i = 0; i < enemySet.enemyAmount; i++)
        {
            SpawnEnemy(enemySet.enemyPrefab);
            Debug.Log($"Spawned Enemy ({i + 1}/{enemySet.enemyAmount})");
            yield return new WaitForSeconds(enemySet.spawnDelay);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.OnDeathEvent += HandleEnemyDeathEvent;
        spawnedEnemies.Add(enemyBase);
        MoveBehaviour moveBehaviour = enemy.GetComponent<MoveBehaviour>();

        if (moveBehaviour != null)
        {
            moveBehaviour.SetTravelPoints(travelPoints);
        }
    }

    private void HandleEnemyDeathEvent(EnemyBase enemyBase)
    {
        enemyBase.OnDeathEvent -= HandleEnemyDeathEvent;
        if (!spawnedEnemies.Remove(enemyBase))
        {
            Debug.LogError("Dead enemy was not spawned by this spawner.");
        }
    
        if (!isSpawning && spawnedEnemies.Count <= 0)
        {
            if (currentWaveIndex >= waves.Count)
            {
                Debug.Log("Game Won!");
                OnGameWin?.Invoke();
            }
            else
            {
                Debug.Log("Wave ended");
                OnWaveEnd?.Invoke();
            }
        }
    }
}
