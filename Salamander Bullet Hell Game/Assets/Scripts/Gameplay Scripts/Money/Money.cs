using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] int value;
    bool deposited = false;
    private void OnEnable() => deposited = false;
    void OnTriggerStay2D(Collider2D other)
    {
        if(!deposited)
        {
            GameMoneyManager.instance.DepositMoney(value);
            deposited = true;
        }
        if(Vector3.Distance(transform.position, other.transform.position) < .4f)
        {
            ObjectPool.ReturnToPool("Money" + value, gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, Time.deltaTime * 10f);
        }
    }

}
