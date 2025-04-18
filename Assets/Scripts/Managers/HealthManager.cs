using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's health and triggers game over when health reaches zero.
/// </summary>
/// <remarks>
/// - Implements `IManager` for global access.
/// - Tracks current health and notifies listeners when health changes.
/// - Invokes `OnGameOver` when health reaches zero.
/// - Provides a method to reduce health (`ReduceLife()`) and retrieve current health.
/// </remarks>

public class HealthManager : MonoBehaviour, IManager
{
    public event Action OnGameOver;
    public event Action<int> OnHealthChanged;

    [SerializeField]
    private int maxHealth = 10;
    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {

    }

    public void ReduceLife()
    {
        currentHealth--;
        Debug.Log($"Health: {currentHealth}");
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
