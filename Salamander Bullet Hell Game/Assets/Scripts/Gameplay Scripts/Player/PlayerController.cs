using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float movementSpeed = 5f;
    [HideInInspector] public float shootForce = 5f;
    [HideInInspector] public bool recoilAttackEnabled = false;
    [HideInInspector] public bool explodingBulletEnabled = false;
    [HideInInspector] public bool shotgunEnabled = false;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject playerRecoilAttack;
    [SerializeField] private int shotgunBulletCount = 3;

    Rigidbody2D rb;
    PlayerDeath playerDeath;

    Vector2 movement;
    bool isRecoiling = false;
    bool movementDisabled = false;
    bool paused = false;
    const float oneOverSqrtTwo = .7071067f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDeath = GetComponent<PlayerDeath>();
    }
    private void OnEnable() => PlayerDeath.playerDeath += DisableMovement;
    private void OnDisable() => PlayerDeath.playerDeath -= DisableMovement;
    void Start()
    {
        if(shotgunEnabled) shootForce = 280f * shotgunBulletCount;
    }

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
        if(shootForce == 0)
        {
            yield break;
        }
        if(recoilAttackEnabled)
        {
            playerRecoilAttack.SetActive(true);
        }
        isRecoiling = true;
        rb.AddForce(-transform.right * shootForce);
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
        if(
            other.gameObject.layer == 9 //normal bullet
        || other.gameObject.layer == 11 //enemy
        || other.gameObject.layer == 13 //bullet not affected by shield
        || other.gameObject.layer == 16 //exploding bullet
        ) 
        {
            playerDeath.Die();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 15) // 15 is explosion layer
        {
            playerDeath.Die();
        }
    }
    private void SpawnBullet()
    {
        GameObject obj;
        if(explodingBulletEnabled)
        {
            List<ExplodingBullet> bullets = new List<ExplodingBullet>();
            if(shotgunEnabled)
            {
                for (int i = 0; i < shotgunBulletCount - 1; i++)
                {
                    Quaternion rotation = Quaternion.Euler(
                        bulletSpawnPoint.transform.rotation.x, 
                        bulletSpawnPoint.transform.rotation.y, 
                        bulletSpawnPoint.transform.eulerAngles.z + Random.Range(-90f, 90f)
                        );
                    obj = ObjectPool.SpawnFromPool("ExplodingBullet", bulletSpawnPoint.transform.position, rotation);
                    bullets.Add(obj.GetComponent<ExplodingBullet>());
                }
            }
            obj = ObjectPool.SpawnFromPool("ExplodingBullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            bullets.Add(obj.GetComponent<ExplodingBullet>());

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
            if(shotgunEnabled)
            {
                for (int i = 0; i < shotgunBulletCount - 1; i++)
                {
                    Quaternion rotation = Quaternion.Euler(
                        bulletSpawnPoint.transform.rotation.x, 
                        bulletSpawnPoint.transform.rotation.y, 
                        bulletSpawnPoint.transform.eulerAngles.z + Random.Range(-90f, 90f)
                        );
                    obj = ObjectPool.SpawnFromPool("PlayerBullet", bulletSpawnPoint.transform.position, rotation);
                }
            }
            obj = ObjectPool.SpawnFromPool("PlayerBullet", bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        }
    }
    private void DisableMovement()
    {
        movementDisabled = true;
        movement = Vector2.zero;
        rb.velocity = Vector2.zero;
    }
}
