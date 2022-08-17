using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : MonoBehaviour, IDestructableObject, IBullet
{
    [SerializeField, Range(0f, 1000f)] float bulletForce = 10f;
    [SerializeField] LayerMask shieldLayer;
    [HideInInspector]public string id;
    [HideInInspector] public List<string> bulletsToIgnore = new List<string>();
    ParticleSystem explosionParticles;
    SpriteRenderer spriteRenderer;
    CircleCollider2D explosionCollider;
    Rigidbody2D rb;
    void Awake()
    {
        id = Guid.NewGuid().ToString();
        explosionCollider = GetComponentInChildren<CircleCollider2D>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        explosionParticles = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0, shieldLayer);
        if(hit.collider != null)
        {
            gameObject.layer = 13;
        }
        else
        {
            gameObject.layer = 16;
        }
        bulletsToIgnore.Remove(id);
        explosionCollider.enabled = false;
        rb.AddForce(transform.right * bulletForce);
    }
    void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ExplodingBullet explodingBullet = other.gameObject.GetComponent<ExplodingBullet>();

        if(explodingBullet != null)
        {
            foreach(string otherID in bulletsToIgnore)
            {
                if(explodingBullet.id == otherID) return;
            }
        }

        if(
            other.gameObject.layer == 11 // enemy layer
        || other.gameObject.layer == 9 //normal bullet layer
        || other.gameObject.layer == 16 //exploding bullet layer
        ) 
        {
            DestroyObject();
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.layer == 16)
        {
            ExplodingBullet explodingBullet = other.gameObject.GetComponent<ExplodingBullet>();

            if(explodingBullet != null)
            {
                bulletsToIgnore.Remove(explodingBullet.id);
                explodingBullet.SetDefaultVelocity();
            }   
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 15 ) // layer 11 is the enemy layer, 15 is explosion layer
        {
            DestroyObject();
        }
    }
    public void DestroyObject()
    {
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        explosionCollider.enabled = true;
        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = false;
        explosionParticles.Play();

        yield return new WaitForSeconds(.25f);


        rb.velocity = Vector2.zero;
        explosionCollider.enabled = false;

        yield return new WaitForSeconds(1.2f);

        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = true;
        ObjectPool.ReturnToPool("ExplodingBullet", gameObject);
    }
    public void SetCorrectLayer()
    {
        gameObject.layer = 16;
    }
    public void SetDefaultVelocity()
    {
        Vector2 direction = rb.velocity.normalized;
        Vector2 dirAndMagnitude = direction * bulletForce;
        rb.velocity = Vector2.zero;
        rb.AddForce(dirAndMagnitude);
    }
    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
    public float GetVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }
}
