using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*public class ScoreManager
{
    public static ScoreManager Instance => _instance ??= new ScoreManager();
    private static ScoreManager _instance;

    public int Score { get; set; }

    protected ScoreManager()
    {
        if (_instance != null)
        {
            throw new Exception("Instance already exists");  
        }
    }
}*/

public class MoneyManager : MonoBehaviour, IManager
{
    public event Action<int> OnMoneyChanged;

     

    [SerializeField]
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
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }
}
