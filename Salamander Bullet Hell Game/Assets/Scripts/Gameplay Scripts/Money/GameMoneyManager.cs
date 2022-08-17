using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameMoneyManager : MonoBehaviour
{
    CurrencyData currencyData;
    public static GameMoneyManager instance;
    public int money {get; private set;}
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currencyData = SaveSystem.ReadCurrencyData();
        if(currencyData == null)
        {
            SaveSystem.SaveCurrencyData(new CurrencyData());
            currencyData = SaveSystem.ReadCurrencyData();
        }
    }

    private void OnEnable() => PlayerDeath.playerDeath += SaveMoney;
    private void OnDisable() => PlayerDeath.playerDeath -= SaveMoney;
    public void DepositMoney(int moneyToDeposit)
    {
        money += moneyToDeposit;
    }
    private void SaveMoney()
    {
        currencyData.money += money;
        SaveSystem.SaveCurrencyData(currencyData);
    }
}
