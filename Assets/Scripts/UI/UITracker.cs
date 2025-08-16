using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages and updates the UI elements showing the player's health, wave number, money, and build phase timer.
/// </summary>
/// <remarks>
/// - Subscribes to events from `MoneyManager`, `HealthManager`, and `GameManager` to update health, money, and wave text.
/// - Updates the build phase timer every frame based on the remaining time from `GameManager`.
/// - Initializes UI values at start and unsubscribes from events on destruction.
/// </remarks>


public class  UITracker : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text buildPhaseTimerText;

    private MoneyManager moneyManager;
    private HealthManager healthManager;
    private GameManager gameManager;

    void Start()
    {
        moneyManager = GameManager.GetManager<MoneyManager>();
        healthManager = GameManager.GetManager<HealthManager>();
        gameManager = GameManager.GetManager<GameManager>(); 
        moneyManager.OnMoneyChanged += UpdateMoneyText;
        healthManager.OnHealthChanged += UpdateHealthText;
        gameManager.OnWaveChanged += UpdateWaveText;

        UpdateHealthText(healthManager.GetCurrentHealth());
        UpdateMoneyText(moneyManager.GetMoney());
        UpdateWaveText(1);
        UpdateBuildPhaseTimer(gameManager.GetBuildPhaseTimeLeft());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBuildPhaseTimer(gameManager.BuildPhaseTimeLeft);
    }

    private void OnDestroy()
    {
        moneyManager.OnMoneyChanged -= UpdateMoneyText;
        healthManager.OnHealthChanged -= UpdateHealthText;
        gameManager.OnWaveChanged -= UpdateWaveText;
    }

    /// <summary>
    /// Updates the money UI when the player's money changes.
    /// </summary>
    /// <param name="money"></param>
    private void UpdateMoneyText(int money)
    {
        moneyText.text = $"$ {money}";
    }

    /// <summary>
    /// Updates the wave UI when the wave number changes.
    /// </summary>
    /// <param name="wave"></param>
    private void UpdateWaveText(int wave)
    {
        waveText.text = $"Wave: {wave}";
    }

    /// <summary>
    /// Updates the health UI when the player's health changes.
    /// </summary>
    /// <param name="health"></param>
    private void UpdateHealthText(int health)
    {
        healthText.text = $"{health}";
    }

    /// <summary>
    /// Updates the timer UI during the build phase.
    /// </summary>
    /// <param name="timeLeft"></param>
    private void UpdateBuildPhaseTimer(float timeLeft)
    {
        buildPhaseTimerText.text = $"{timeLeft:0}";
    }
}
