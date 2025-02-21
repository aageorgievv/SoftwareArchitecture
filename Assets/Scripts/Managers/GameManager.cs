using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum EGameState
    {
        None,
        BuildingPhase,
        CombatPhase
    }
    private EGameState currentGameState = EGameState.None;

    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private float buildPhaseDuration = 10f;

    void Start()
    {
        StartBuildPhase();
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        if (EnemySpawner.Instance == null)
        {
            Debug.LogError("EnemySpawner instance is null");
            return;
        }
        if (HealthManager.Instance == null)
        {
            Debug.LogError("HealthManager instance is null");
            return;
        }

        EnemySpawner.Instance.OnWaveEnd += StartBuildPhase;
        HealthManager.Instance.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        EnemySpawner.Instance.OnWaveEnd -= StartBuildPhase;
        HealthManager.Instance.OnGameOver -= HandleGameOver;
    }

    private void StartBuildPhase()
    {
        currentGameState = EGameState.BuildingPhase;
        Debug.Log("GameState: BuildPhase");
        StartCoroutine(BuildPhaseCountDown());
    }

    private IEnumerator BuildPhaseCountDown()
    {
        yield return new WaitForSeconds(buildPhaseDuration);
        StartCombatPhase();
    }

    private void StartCombatPhase()
    {
        currentGameState = EGameState.CombatPhase;
        Debug.Log("GameState: CombatPhase");
        enemySpawner.StartNextWave();
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over!");
    }
}
