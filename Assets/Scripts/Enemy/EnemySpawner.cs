using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawns enemies based on WaveConfig(Scriptable Object - Containing EnemySet info) 
    //Controls delay between enemy spawns
    // Fires an event when a wave ends
    public static event Action OnWaveEnd;

    [SerializeField]
    private List<WaveConfig> waves;
    [SerializeField] 
    private List<Transform> travelPoints;
    [SerializeField]
    private Transform spawnPoint;

    private int currentWaveIndex = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
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
        // Notify GameManager when the entire wave ends
        OnWaveEnd?.Invoke();
        Debug.Log("End");
    }

    private IEnumerator spawnEnemySet(EnemySet enemySet)
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
        MoveBehaviour moveBehaviour = enemy.GetComponent<MoveBehaviour>();

        if (moveBehaviour != null)
        {
            moveBehaviour.SetTravelPoints(travelPoints);
        }
    }
}
