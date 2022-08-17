using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecoilAttack : MonoBehaviour
{
    [SerializeField] private float smoothing;
    Rigidbody2D rb;
    void Awake()
    {
        rb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        // Animations and stuff go here
    }
    void OnDisable()
    {
        // Animations and stuff go here
    }
    void Update()
    {
        float scaleTarget = rb.velocity.magnitude * .15f;

        float scale = Mathf.Lerp(transform.localScale.y, scaleTarget, Time.deltaTime * smoothing);
        transform.localScale = new Vector3(.15f, scale, 1f);

        if(rb.velocity.sqrMagnitude < 15f)
        {
            StartCoroutine(DisableShield());
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if((other.gameObject.transform.position - transform.position).sqrMagnitude < .003f * rb.velocity.sqrMagnitude)
        {
            IDestructableObject destructableObject = other.gameObject.GetComponent<IDestructableObject>();
            destructableObject.DestroyObject();
        }
    }

    float timer = .5f;
    IEnumerator DisableShield()
    {
        while(timer > 0)
        {
            // Make sure we don't cancel the recoil shield while still moving.
            if(rb.velocity.sqrMagnitude > 15f)
            {
                yield break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = .5f;
        gameObject.SetActive(false);
    }
}
