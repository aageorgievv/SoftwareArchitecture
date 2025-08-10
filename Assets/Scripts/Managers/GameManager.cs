using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Central controller for game flow, managing states, timers, dependencies, and events.
/// </summary>
/// <remarks>
/// - Implements `IManager` and registers itself and other managers in a global lookup dictionary.
/// - Controls game state transitions between the building and combat phases.
/// - Runs a countdown timer for the build phase and automatically starts the next wave.
/// - Keeps track of the current wave number and remaining build phase time.
/// - Listens for wave end, game over, and game win events to trigger state changes or UI updates.
/// - Adjusts the game’s time scale via a UI slider.
/// - Provides global access to registered managers through `GetManager<T>()`.
/// - Unit test for Triggering Game Over instantly from the inspector vie [ContextMenu("Trigger Game Over")]""
/// </remarks>


public interface IManager
{

}

public class GameManager : MonoBehaviour, IManager
{
    public enum EGameState
    {
        None,
        BuildingPhase,
        CombatPhase
    }

    public EGameState CurrentGameState { get; private set; } = EGameState.None;

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

    [SerializeField]
    private Slider timeScaleSlider;

    [SerializeField] private WinScreenUI winScreenUI;

    private static Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

    private int waveNumber = 1;
    public float BuildPhaseTimeLeft { get; private set; }

    public event Action<int> OnWaveChanged;
    public event Action OnGameOverEvent;

    private Coroutine previousCoroutine;

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

        if (timeScaleSlider == null)
        {
            Debug.LogError($"{nameof(timeScaleSlider)} is null");
        }

        managers.Clear();
        managers.Add(typeof(GameManager), this);
        managers.Add(typeof(HealthManager), healthManager);
        managers.Add(typeof(TextDisplayManager), textDisplayManager);
        managers.Add(typeof(MoneyManager), moneyManager);
        managers.Add(typeof(TowerSelectionManager), selectionManager);

        enemySpawner.OnWaveEnd += StartBuildPhase;
        healthManager.OnGameOver += HandleGameOver;
        enemySpawner.OnGameWin += HandleGameWin;

        if(timeScaleSlider != null)
        {
            timeScaleSlider.onValueChanged.AddListener(HandleTimeScaleValueChanged);

        }
        Time.timeScale = 1f; // reset timescale
    }

    private void Start()
    {
        StartBuildPhase();
    }

    public static T GetManager<T>() where T : IManager
    {
        return (T)managers[typeof(T)];
    }

    private void OnDestroy()
    {
        enemySpawner.OnWaveEnd -= StartBuildPhase;
        healthManager.OnGameOver -= HandleGameOver;
        enemySpawner.OnGameWin -= HandleGameWin;
        timeScaleSlider.onValueChanged.RemoveListener(HandleTimeScaleValueChanged);
    }

    private void StartBuildPhase()
    {
        Debug.Log("GameState: BuildPhase");
        CurrentGameState = EGameState.BuildingPhase;

        if (previousCoroutine != null)
        {
            StopCoroutine(previousCoroutine);
        }

        previousCoroutine = StartCoroutine(BuildPhaseCountDown());
    }

    private IEnumerator BuildPhaseCountDown()
    {
        BuildPhaseTimeLeft = buildPhaseDuration;

        while (BuildPhaseTimeLeft > 0)
        {
            BuildPhaseTimeLeft -= Time.deltaTime;
            yield return null;
        }

        StartCombatPhase();
    }

    private void StartCombatPhase()
    {
        Debug.Log("GameState: CombatPhase");
        CurrentGameState = EGameState.CombatPhase;
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

    private void HandleTimeScaleValueChanged(float value)
    {
        Time.timeScale = value;
        Debug.Log($"TimeScale: {value}");
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetBuildPhaseTimeLeft()
    {
        return BuildPhaseTimeLeft;
    }

    public bool IsInBuildingPhase()
    {
        return CurrentGameState == EGameState.BuildingPhase;
    }

    [ContextMenu("Trigger Game Over")]
    public void TriggerGameOver()
    {
        healthManager.TriggerGameOver();
        Time.timeScale = 0f;
    }
}
