using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour, IManager
{
    public event Action<int> OnMoneyChanged;

    private int money = 500;

    public int GetMoney()
    {
        return money;
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    public void SpendMoney(int amount)
    {
        if(CanAfford(amount))
        {
            money -= amount;
            OnMoneyChanged?.Invoke(money);
        } else
        {
            Debug.Log("You broke! Money lacking!");
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }
}
