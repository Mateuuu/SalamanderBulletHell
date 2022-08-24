using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static event Action playerIsHit;



    #region Player Property Variables

    // * Variables that affect the player's properties

    // * Movement Variables
    [HideInInspector] public float movementSpeed;
    // * Recoil Variables
    [HideInInspector] public float shootForce;
    // * Bullet Variables
    [HideInInspector] public float explodingBullet;
    [HideInInspector] public int shotgun;
    [HideInInspector] public float bulletSpeed;
    [HideInInspector] public float bulletSize;
    [HideInInspector] public bool bulletTrailActivated = false;

    [HideInInspector] public bool recoilAttackEnabled = false;

    #endregion

    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject playerRecoilAttack;
    [SerializeField] GameObject instakillEverything;

    Rigidbody2D rb;
    SpriteRenderer sr;
    PlayerDeath playerDeath;
    PlayerBulletTrail playerBulletTrail;

    Vector2 movement;


    bool isRecoiling = false;
    bool movementDisabled = false;
    bool paused = false;
    bool invincibilityFrames = false;


    const float oneOverSqrtTwo = .7071067f;
    void Awake()
    {
        instakillEverything.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerBulletTrail = GetComponent<PlayerBulletTrail>();
    }
    private void OnEnable() => PlayerDeath.playerDeath += DisableMovement;
    private void OnDisable() => PlayerDeath.playerDeath -= DisableMovement;

    void Update()
    {
        if(movementDisabled)
        {
            return;
        }
        if(!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            Time.timeScale = 0f;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = false;
            Time.timeScale = 1f;
        }
        if(paused) return;



        // Right Click
        if(Input.GetKey(KeyCode.Mouse1))
        {
            // enable right click effects
            if(bulletTrailActivated)
            {
                playerBulletTrail.ActivateTrail();
            }
        }
        // disable the right click effects
        else
        {
            playerBulletTrail.DeactivateTrail();
        }



        // Left Click
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnBullet();
            StartCoroutine(Recoil());
        }
        movement.y = Input.GetAxisRaw("Vertical");
        movement.x = Input.GetAxisRaw("Horizontal");
        Rotate(GetAngleToMouse());
    }
    void FixedUpdate()
    {
        float diagonalSpeedMultiplier = 1;

        if(movement.x != 0 && movement.y != 0)
        {
            diagonalSpeedMultiplier = oneOverSqrtTwo;
        }
        if(!isRecoiling)
        {
            rb.velocity = movement * Time.deltaTime * movementSpeed * diagonalSpeedMultiplier;
        }
    }
    WaitForFixedUpdate fixedUpdateWait = new WaitForFixedUpdate();
    IEnumerator Recoil()
    {
        if(shootForce == 0 && shotgun == 0)
        {
            yield break;
        }
        if(recoilAttackEnabled)
        {
            playerRecoilAttack.SetActive(true);
        }

        float tempShootForce;
        tempShootForce = shootForce + ((float)shotgun * 200f);


        isRecoiling = true;
        rb.AddForce(-transform.right * tempShootForce);
        yield return fixedUpdateWait;
        while(rb.velocity.sqrMagnitude > 30f)
        {
            yield return fixedUpdateWait;
        }
        isRecoiling = false;
    }
    private float GetAngleToMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        return angle;
    }
    private void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(invincibilityFrames) return;
        if(
            other.gameObject.layer == 9 //normal bullet
        || other.gameObject.layer == 11 //enemy
        || other.gameObject.layer == 13 //bullet not affected by shield
        || other.gameObject.layer == 16 //exploding bullet
        ) 
        {
            StartCoroutine(TakeDamage());
            StartCoroutine(InstaKillEverything());
            playerIsHit?.Invoke();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(invincibilityFrames) return;

        if(other.gameObject.layer == 15) // 15 is explosion layer
        {
            StartCoroutine(TakeDamage());
            StartCoroutine(InstaKillEverything());
            playerIsHit?.Invoke();
        }
    }
    private void SpawnBullet()
    {
        GameObject obj;
        ExplodingBullet tempExplodingBullet;
        PlayerBullet tempBullet;
        if(explodingBullet != 0)
        {
            List<ExplodingBullet> bullets = new List<ExplodingBullet>();
            if(shotgun != 0)
            {
                for (int i = 0; i < shotgun - 1; i++)
                {
                    Quaternion rotation = Quaternion.Euler(
                        bulletSpawnPoint.transform.rotation.x, 
                        bulletSpawnPoint.transform.rotation.y, 
                        bulletSpawnPoint.transform.eulerAngles.z + UnityEngine.Random.Range(-90f, 90f)
                        );
                    obj = ObjectPool.SpawnFromPool("ExplodingBullet", bulletSpawnPoint.transform.position, rotation);
                    tempExplodingBullet = obj.GetComponent<ExplodingBullet>();
                    // Change the properties of the bullet
                    tempExplodingBullet.SetSpeed(bulletSpeed);
                    tempExplodingBullet.SetSize(bulletSize);
                    tempExplodingBullet.Shoot();



                    bullets.Add(tempExplodingBullet);
                }
            }
            obj = ObjectPool.SpawnFromPool("ExplodingBullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            tempExplodingBullet = obj.GetComponent<ExplodingBullet>();
            // Change the properties of the bullet
            tempExplodingBullet.SetSpeed(bulletSpeed);
            tempExplodingBullet.SetSize(bulletSize);
            tempExplodingBullet.Shoot();


            bullets.Add(tempExplodingBullet);

            foreach(ExplodingBullet bullet in bullets)
            {
                bullet.bulletsToIgnore.Clear();
                foreach(ExplodingBullet otherBullet in bullets)
                {
                    bullet.bulletsToIgnore.Add(otherBullet.id);
                }
            }
        }
        else
        {
            if(shotgun != 0)
            {
                for (int i = 0; i < shotgun - 1; i++)
                {
                    Quaternion rotation = Quaternion.Euler(
                        bulletSpawnPoint.transform.rotation.x, 
                        bulletSpawnPoint.transform.rotation.y, 
                        bulletSpawnPoint.transform.eulerAngles.z + UnityEngine.Random.Range(-90f, 90f)
                        );
                    obj = ObjectPool.SpawnFromPool("PlayerBullet", bulletSpawnPoint.transform.position, rotation);
                    tempBullet = obj.GetComponent<PlayerBullet>();
                    tempBullet.SetSpeed(bulletSpeed);
                    tempBullet.SetSize(bulletSize);
                    tempBullet.Shoot();
                }
            }
            obj = ObjectPool.SpawnFromPool("PlayerBullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            tempBullet = obj.GetComponent<PlayerBullet>();
            tempBullet.SetSpeed(bulletSpeed);
            tempBullet.SetSize(bulletSize);
            tempBullet.Shoot(); 
        }
    }
    private void DisableMovement()
    {
        movementDisabled = true;
        movement = Vector2.zero;
        rb.velocity = Vector2.zero;
    }
    IEnumerator InstaKillEverything()
    {
        instakillEverything.SetActive(true);
        yield return fixedUpdateWait;
        yield return fixedUpdateWait;
        yield return fixedUpdateWait;
        yield return fixedUpdateWait;
        yield return fixedUpdateWait;
        instakillEverything.SetActive(false);
    }
    IEnumerator TakeDamage()
    {
        invincibilityFrames = true;
        for(int i = 0; i < 30; i++)
        {
            if(i % 2 == 0)
            {
                sr.color = Color.red;
                yield return fixedUpdateWait;
            }
            else
            {
                sr.color = Color.white;
                yield return fixedUpdateWait;
            }
        }
        invincibilityFrames = false;

    }
}
