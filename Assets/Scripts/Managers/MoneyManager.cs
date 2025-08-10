using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's in-game currency.
/// </summary>
/// <remarks>
/// - Implements `IManager` for global access.
/// - Tracks available money and triggers `OnMoneyChanged` event when updated.
/// - Provides methods to check affordability (`CanAfford()`), spend (`SpendMoney()`), and earn (`AddMoney()`) money.
/// - Ensures that money is only deducted if the player can afford the cost.
/// </remarks>

public class MoneyManager : MonoBehaviour, IManager
{
    public event Action<int> OnMoneyChanged;

    [SerializeField]
    private int money = 500;
    [SerializeField] bool infiniteMoney = false;

    public int GetMoney()
    {
        if (infiniteMoney)
        {
            return int.MaxValue;
        }
        return money;
    }

    public bool CanAfford(int amount)
    {
        if (infiniteMoney)
        {
            return true;
        }
        return money >= amount;
    }

    public void SpendMoney(int amount)
    {
        if (infiniteMoney)
        {
            OnMoneyChanged?.Invoke(int.MaxValue);
            return;
        }

        if (CanAfford(amount))
        {
            money -= amount;
            OnMoneyChanged?.Invoke(money);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }
}
