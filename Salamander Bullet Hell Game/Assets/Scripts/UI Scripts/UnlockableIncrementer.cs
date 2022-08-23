using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnlockableIncrementer : Incrementer
{
    [SerializeField] ShopMoneyManager shopMoneyManager;
    [SerializeField] Image purchaseDisplay;
    [SerializeField] private int[] prices;
    [System.Serializable] public class OnLevelChanged : UnityEvent<int> { };
     public OnLevelChanged onLevelChanged;
    private int purchased;
    public void TryIncrement()
    {
        if(purchased > incrementStatus)
        {
            base.Increment();
            return;
        }
        if(incrementStatus >= prices.Length) return;
        if(prices[incrementStatus] > shopMoneyManager.currencyData.money) return;
        if(purchased >= numOfIncrements) return;
        purchased++;
        onLevelChanged.Invoke(purchased);
        shopMoneyManager.currencyData.money -= prices[incrementStatus];
        shopMoneyManager.UpdateDisplay();

        purchased = Mathf.Clamp(purchased, 0, numOfIncrements);
        purchaseDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(purchased * imagePixelsPerUnit, imagePixelsPerUnit);
        base.Increment();
    }
    public void TryDecrement()
    {
        base.Decrement();
    }
    protected override void AddButtonListeners()
    {
        increaseButton.onClick.AddListener(TryIncrement);
        decreaseButton.onClick.AddListener(TryDecrement);
    }
    protected override void RemoveButtonListeners()
    {
        increaseButton.onClick.RemoveListener(TryIncrement);
        decreaseButton.onClick.RemoveListener(TryDecrement);
    }
    protected override void Awake()
    {
        purchased = Mathf.Clamp(purchased, 0, numOfIncrements);
        if(prices.Length < numOfIncrements) Debug.LogWarning("Prices length is less than number of increments");
        purchaseDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(purchased * imagePixelsPerUnit, imagePixelsPerUnit);
        base.Awake();
    }
    /// <summary>
    /// Method <c>SetValue</c> directly sets the value. Only use if you wish to override current value
    /// <returns>
    /// Increment Status 
    /// </returns>
    /// </summary>
    public int SetValue(float value, int level)
    {
        purchased = level;
        purchased = Mathf.Clamp(purchased, 0, numOfIncrements);
        purchaseDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(purchased * imagePixelsPerUnit, imagePixelsPerUnit);
        return base.SetValue(value);
    }
    /// <summary>
    /// Method <c>SetValue</c> directly sets the increment. Only use if you wish to override current increment
    /// <returns>
    /// Value 
    /// </returns>
    /// </summary>
    public float SetIncrement(int increment)
    {
        increment = Mathf.Clamp(increment, 0, numOfIncrements);
        incrementStatus = increment;
        incrementImageTransform.sizeDelta = new Vector2(increment * imagePixelsPerUnit, imagePixelsPerUnit);

        // y = mx + b
        float m = (maxValue - minValue)/(float)numOfIncrements;
        float x = (float)incrementStatus;
        float b = minValue;
        value = (m*x) + b;

        return value;
    }
}
