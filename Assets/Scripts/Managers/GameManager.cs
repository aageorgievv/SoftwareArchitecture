using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{

}

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
    private HealthManager healthManager;

    [SerializeField]
    private TextDisplayManager textDisplayManager;

    [SerializeField]
    private MoneyManager moneyManager;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private TowerSelectionManager selectionManager;
    [SerializeField]
    private float buildPhaseDuration = 10f;

    private static Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

    private void Awake()
    {
        if(healthManager == null)
        {
            Debug.LogError("HealthManager is null");
        }

        if (textDisplayManager == null)
        {
            Debug.LogError("TextDisplayManager is null");
        }

        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is null");
        }

        if (selectionManager == null)
        {
            Debug.LogError("TowerSelectionManager is null");
        }

        managers.Add(typeof(HealthManager), healthManager);
        managers.Add(typeof(TextDisplayManager), textDisplayManager);
        managers.Add(typeof(MoneyManager), moneyManager);
        managers.Add(typeof(TowerSelectionManager), selectionManager);

        enemySpawner.OnWaveEnd += StartBuildPhase;
        healthManager.OnGameOver += HandleGameOver;
    }

    public static T GetManager<T>() where T : IManager
    {
        return (T)managers[typeof(T)];
    }

    void Start()
    {
        StartBuildPhase();
    }

    private void OnDestroy()
    {
        enemySpawner.OnWaveEnd -= StartBuildPhase;
        healthManager.OnGameOver -= HandleGameOver;
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
