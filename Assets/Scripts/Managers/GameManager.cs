using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager
{

}

public class GameManager : MonoBehaviour, IManager
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
    private TowerSelectionManager selectionManager;
    [SerializeField]
    private float buildPhaseDuration = 10f;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField] private WinScreenUI winScreenUI;

    private static Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

    private int waveNumber = 1;
    private float buildPhaseTimeLeft;

    public event Action<int> OnWaveChanged;
    public event Action<float> OnBuildPhaseTimeChanged;
    public event Action OnGameOverEvent;


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

        managers.Add(typeof(GameManager), this);
        managers.Add(typeof(HealthManager), healthManager);
        managers.Add(typeof(TextDisplayManager), textDisplayManager);
        managers.Add(typeof(MoneyManager), moneyManager);
        managers.Add(typeof(TowerSelectionManager), selectionManager);

        enemySpawner.OnWaveEnd += StartBuildPhase;
        healthManager.OnGameOver += HandleGameOver;
        enemySpawner.OnGameWin += HandleGameWin;
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
        enemySpawner.OnGameWin -= HandleGameWin;
    }

    private void StartBuildPhase()
    {
        currentGameState = EGameState.BuildingPhase;
        Debug.Log("GameState: BuildPhase");
        StartCoroutine(BuildPhaseCountDown());
    }

    private IEnumerator BuildPhaseCountDown()
    {
        buildPhaseTimeLeft = buildPhaseDuration;

        while (buildPhaseTimeLeft > 0)
        {
            OnBuildPhaseTimeChanged?.Invoke(buildPhaseTimeLeft);
            yield return new WaitForSeconds(1f);
            buildPhaseTimeLeft--;
        }

        StartCombatPhase();
    }

    private void StartCombatPhase()
    {
        currentGameState = EGameState.CombatPhase;
        Debug.Log("GameState: CombatPhase");
        enemySpawner.StartNextWave();
        waveNumber++;
        OnWaveChanged?.Invoke(waveNumber);
    }

    private void HandleGameOver()
    {
        OnGameOverEvent?.Invoke();
    }

    private void HandleGameWin()
    {
        Debug.Log("You Win!");
        winScreenUI.ShowWinScreen();
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetBuildPhaseTimeLeft()
    {
        return buildPhaseTimeLeft;
    }
}
