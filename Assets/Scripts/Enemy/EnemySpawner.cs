using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawning of enemies in waves and manages their lifecycle.
/// </summary>
/// <remarks>
/// - Spawns multiple waves of enemies defined by a list of WaveConfig.
/// - Each wave consists of one or more enemy sets EnemySettings that spawn enemies with delays and cooldowns.
/// - Spawning is handled asynchronously using coroutines to allow timed spawning of enemies and sets.
/// - Assigns predefined travel points to enemies via their MoveBehaviour component after instantiation.
/// - Tracks all spawned enemies and listens for their death events to update active enemy count.
/// - Raises OnWaveEnd event when all enemies of the current wave are defeated and no more are spawning.
/// - Raises OnGameWin event when all waves have been completed and all enemies defeated.
/// - Logs detailed debug information about spawning progress and state changes.
/// - Toggle for instantKillEnabled for whenever the enemies get shot by a tower
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
    [SerializeField] private bool instantKillEnabled = false;

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

    /// <summary>
    /// Begins spawning the next wave of enemies if there are any waves remaining.
    /// </summary>
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


    /// <summary>
    /// Coroutine that spawns all enemy sets in a given wave with delays between sets.
    /// </summary>
    /// <param name="waveConfig"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Coroutine that spawns a specific set of enemies with a delay between each enemy.
    /// </summary>
    /// <param name="enemySet"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Instantiates a single enemy and assigns travel points.
    /// </summary>
    /// <param name="enemyPrefab"></param>
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.InstantKill = instantKillEnabled;
        enemyBase.OnDeathEvent += HandleEnemyDeathEvent;
        spawnedEnemies.Add(enemyBase);
        MoveBehaviour moveBehaviour = enemy.GetComponent<MoveBehaviour>();

        if (moveBehaviour != null)
        {
            moveBehaviour.SetTravelPoints(travelPoints);
        }
    }

    /// <summary>
    /// Handles cleanup when an enemy dies and triggers wave or game completion events if applicable.
    /// </summary>
    /// <param name="enemyBase"></param>
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

    /// <summary>
    /// Toggles instant kill for all currently spawned enemies.
    /// </summary>
    /// <param name="state"></param>
    public void SetInstantKillEnabled(bool state)
    {
        instantKillEnabled = enabled;

        foreach (var enemy in spawnedEnemies)
        {
            enemy.InstantKill = instantKillEnabled;
        }
    }
}
