using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum EGameState
    {
        BuildingPhase,
        CombatPhase
    }
    private EGameState currentGameState;

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
        EnemySpawner.OnWaveEnd += StartBuildPhase;
    }

    private void OnDisable()
    {
        EnemySpawner.OnWaveEnd -= StartBuildPhase;
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
}
