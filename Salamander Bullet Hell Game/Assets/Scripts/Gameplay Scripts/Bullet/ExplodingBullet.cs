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
    [SerializeField] Transform explosion;
    ParticleSystem explosionParticles;
    SpriteRenderer spriteRenderer;
    CircleCollider2D explosionCollider;
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    bool exploded = false;
    void Awake()
    {
        
        boxCollider2D = GetComponent<BoxCollider2D>();
        id = Guid.NewGuid().ToString();
        explosionCollider = GetComponentInChildren<CircleCollider2D>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        explosionParticles = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
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
        if(other.gameObject.layer == 15  || other.gameObject.layer == 19) // layer 11 is the enemy layer, 15 is explosion layer
        {
            if(!exploded)
            {
                DestroyObject();
            }
        }
    }
    public void DestroyObject()
    {
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        exploded = true;

        boxCollider2D.enabled = true;
        explosionCollider.enabled = true;
        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = false;
        explosionParticles.Play();

        yield return new WaitForSeconds(.25f);

        rb.velocity = Vector2.zero;
        explosionCollider.enabled = false;
        boxCollider2D.enabled = false;

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
    public void SetSize(float size)
    {
        size = Mathf.Clamp(size, 0f, 1f) + .3f;
        transform.localScale = new Vector3(size, size, size);
        float explosionSize = Mathf.Clamp(size, 0f, 1f) + 1f;
        explosion.localScale = new Vector3(explosionSize, explosionSize, explosionSize);
    }
    public void SetSpeed(float speed)
    {
        speed = Mathf.Clamp(speed, 0f, 1f) + 1f;
        // 300 is the default force value for exploding bullets
        bulletForce = 300f * speed;
    }
    public void Shoot()
    {
        boxCollider2D.enabled = true;
        exploded = false;
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
}
