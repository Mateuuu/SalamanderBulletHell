using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopMoneyManager : MonoBehaviour
{
    public CurrencyData currencyData;
    
    [SerializeField] TMP_Text textDisplay;
    void Awake()
    {
        currencyData = SaveSystem.ReadCurrencyData();
        if(currencyData == null)
        {
            SaveSystem.SaveCurrencyData(new CurrencyData());
            currencyData = SaveSystem.ReadCurrencyData();
        }
        textDisplay.text = "Money: " + currencyData.money.ToString();
    }
    public void UpdateDisplay()
    {
        textDisplay.text = "Money: " + currencyData.money.ToString();
    }
}
