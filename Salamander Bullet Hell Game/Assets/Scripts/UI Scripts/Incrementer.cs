using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Incrementer : MonoBehaviour
{
     [System.Serializable] public class OnValueChanged : UnityEvent<float> { };
     public OnValueChanged onValueChanged;
    [SerializeField] protected Button increaseButton;
    [SerializeField] protected Button decreaseButton;
    [SerializeField] Image  incrementImage;
    [SerializeField] Image  backgroundImage;
    [SerializeField] protected int imagePixelsPerUnit;
    [SerializeField] protected float minValue;
    [SerializeField] protected float maxValue;
    [SerializeField] protected int numOfIncrements;
    protected RectTransform  incrementImageTransform;
    protected float value = 0f;
    public int incrementStatus {get; protected set;}
    protected virtual void Awake()
    {
        incrementImageTransform = incrementImage.GetComponent<RectTransform>();
        backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2(numOfIncrements * imagePixelsPerUnit, imagePixelsPerUnit);
        incrementImageTransform.sizeDelta = new Vector2(0 * imagePixelsPerUnit, imagePixelsPerUnit);
        AddButtonListeners();

    }
    private void OnDestroy()
    {
        RemoveButtonListeners();
    }
    protected virtual void AddButtonListeners()
    {
        increaseButton.onClick.AddListener(Increment);
        decreaseButton.onClick.AddListener(Decrement);
    }
    protected virtual void RemoveButtonListeners()
    {
        increaseButton.onClick.RemoveListener(Increment);
        decreaseButton.onClick.RemoveListener(Decrement);
    }
    public float GetValue()
    {
        return value;
    }
    /// <summary>
    /// Method <c>SetValue</c> directly sets the value. Only use if you wish to override current value
    /// Returns:
    /// Increment Status
    /// </summary>
    public virtual int SetValue(float value)
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        // inverse of y = mx + b
        float m = (maxValue - minValue)/(float)numOfIncrements;
        float x = value;
        float b = minValue;
        incrementStatus = Mathf.RoundToInt((x - b)/m);


        incrementImageTransform.sizeDelta = new Vector2(incrementStatus * imagePixelsPerUnit, imagePixelsPerUnit);
        return incrementStatus;
    }
    protected void Increment()
    {
        incrementStatus += 1;
        if(incrementStatus > numOfIncrements - 1)
        {
            incrementStatus = numOfIncrements;
            value = maxValue;
            incrementImageTransform.sizeDelta = new Vector2(numOfIncrements * imagePixelsPerUnit, imagePixelsPerUnit);
            onValueChanged.Invoke(value);
            return;
        }
        incrementImageTransform.sizeDelta = new Vector2(incrementStatus * imagePixelsPerUnit, imagePixelsPerUnit);

        // y = mx + b
        float m = (maxValue - minValue)/numOfIncrements;
        float x = incrementStatus;
        value = (m*x) + minValue;

        onValueChanged.Invoke(value);
    }
    protected void Decrement()
    {
        incrementStatus -= 1;
        if(incrementStatus <= 0)
        {
            incrementStatus = 0;
            value = minValue;
            incrementImageTransform.sizeDelta = new Vector2(0 * imagePixelsPerUnit, imagePixelsPerUnit);
            onValueChanged.Invoke(value);
            return;
        }
        incrementImageTransform.sizeDelta = new Vector2(incrementStatus * imagePixelsPerUnit, imagePixelsPerUnit);

        // y = mx + b
        float m = (maxValue - minValue)/(float)numOfIncrements;
        float x = (float)incrementStatus;
        float b = minValue;
        value = (m*x) + b;


        onValueChanged.Invoke(value);
    }
}
