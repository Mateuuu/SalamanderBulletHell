using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBullet : MonoBehaviour, IDestructableObject, IBullet
{
    [SerializeField, Range(0f, 1000f)] float bulletForce = 10f;
    [SerializeField] LayerMask shieldLayer;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(
            other.gameObject.layer == 11 
        || other.gameObject.layer == 9
        ) // layer 11 is the enemy layer, 14 is recoil shield
        {
            ObjectPool.ReturnToPool("PlayerBullet", gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 15 || other.gameObject.layer == 19) //15 is explosion layer
        {
            DestroyObject();
        }
    }
    public void DestroyObject()
    {
            ObjectPool.ReturnToPool("PlayerBullet", gameObject);
    }
    public void SetCorrectLayer()
    {
        gameObject.layer = 9;
    }
    public void SetDefaultVelocity()
    {
        Vector2 direction = rb.velocity.normalized;
        Vector2 dirAndMagnitude = direction * bulletForce;
        rb.velocity = Vector2.zero;
        rb.AddForce(dirAndMagnitude);
    }
    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }
    public void SetSize(float size)
    {
        size = Mathf.Clamp(size, 0f, 1f) + .3f;
        transform.localScale = new Vector3(size, size, size);
    }
    public void SetSpeed(float speed)
    {
        speed = Mathf.Clamp(speed, 0f, 1f) + 1;
        // 500 is the default force value for normal bullets
        bulletForce = 500f * speed;
    }
    public void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0, shieldLayer);
        if(hit.collider != null)
        {
            gameObject.layer = 13;
        }
        else
        {
            gameObject.layer = 9;
        }
        rb.AddForce(transform.right * bulletForce);
    }
}
