using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrontShield : MonoBehaviour
{
    private float scale;

    public void SetScale(float scale)
    {
        this.scale = scale;
    }
    void OnEnable()
    {
        transform.localScale = new Vector3(.15f, scale, 1f);
        // Animations and stuff go here
    }
    void OnDisable()
    {
        // Animations and stuff go here
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 11)
        {
            return;
        }
        IDestructableObject destructableObject = other.gameObject.GetComponent<IDestructableObject>();
        destructableObject.DestroyObject();
    }
    public void DisableShield()
    {
        gameObject.SetActive(false);
    }
    public void EnableShield()
    {
        gameObject.SetActive(true);
    }

}
