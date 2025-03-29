using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
