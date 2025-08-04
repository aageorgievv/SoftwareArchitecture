using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the overall game state, including build and combat phases.
/// </summary>
/// <remarks>
/// - Implements `IManager` and provides a static manager lookup system.
/// - Handles game state transitions between `BuildingPhase` and `CombatPhase`.
/// - Manages dependencies like `HealthManager`, `MoneyManager`, and `EnemySpawner`.
/// - Handles UI interactions such as time scaling and starting waves.
/// - Listens for game events like wave completion, game over, and game win.
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

        timeScaleSlider.onValueChanged.AddListener(HandleTimeScaleValueChanged);

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
}
