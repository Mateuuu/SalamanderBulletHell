using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] int value;
    bool deposited = false;
    bool following = false;
    private void OnEnable()
    {
        deposited = false;
        following = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(following) return;
        StartCoroutine(FollowPlayer(other.transform));
    }
    WaitForFixedUpdate fixedUpdateWait = new WaitForFixedUpdate();
    IEnumerator FollowPlayer(Transform target)
    {
        following = true;
        while(Vector3.Distance(transform.position, target.position) > .4f)
        {
            yield return fixedUpdateWait;
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 10f);
        }
        if(!deposited)
        {
            GameMoneyManager.instance.DepositMoney(value);
            deposited = true;
        }
        ObjectPool.ReturnToPool("Money" + value, gameObject);

    }

}
