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

    [SerializeField] private Button startButton;
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

        if (timeScaleSlider == null)
        {
            Debug.LogError($"{nameof(timeScaleSlider)} is null");
        }

        if (startButton == null)
        {
            Debug.LogError($"{nameof(startButton)} is null");
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

        startButton.onClick.AddListener(HandleStartButtonClicked);
        timeScaleSlider.onValueChanged.AddListener(HandleTimeScaleValueChanged);

        Time.timeScale = 1f; // reset timescale
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
        startButton.onClick.RemoveListener(HandleStartButtonClicked);
    }

    private void StartBuildPhase()
    {
        CurrentGameState = EGameState.BuildingPhase;
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
        CurrentGameState = EGameState.CombatPhase;
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
    private void HandleStartButtonClicked()
    {
        StartBuildPhase();
    }

    private void HandleTimeScaleValueChanged(float value)
    {
        Time.timeScale = value;
        Debug.Log($"TimeScale: {value}");
    }
}
