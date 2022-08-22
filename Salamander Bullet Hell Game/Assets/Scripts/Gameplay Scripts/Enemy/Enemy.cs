using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDestructableObject
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] public EnemyData enemyData;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] Transform movementTransform;
    [SerializeField] EnemyFrontShield enemyFrontShield;
    [SerializeField] EnemyShield enemyShield;
    Transform playerTransform;
    Rigidbody2D rb;
    Transform parentTransform;
    private float recoilForce;
    private bool isRecoiling;
    bool destroyed = false;
    void Awake()
    {
        parentTransform = transform.parent;
        playerTransform = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        destroyed = false;
    }
    public void ResetData()
    {
        /* Changing the variables during runtime. 
        That way all you have to do is change the SO 
        of the enemy and it has entirely different behavior. */
        enemyFrontShield.SetScale(enemyData.frontShieldSize * 3f);
        enemyFrontShield.EnableShield();

        enemyShield.EnableShieldDeflect(enemyData.shieldBounceEnabled);
        enemyShield.SetShieldRadius(enemyData.shieldRadius * 10f);
        enemyShield.SetShieldSlowdown(enemyData.shieldSlowdown);
        enemyShield.SetBounceProbability(enemyData.shieldBounceProbability);

        if(!enemyData.frontShieldEnabled)
        {
            enemyFrontShield.DisableShield();
        }

        agent.speed = enemyData.movementSpeed * 30;
        if(enemyData.fireRate != 0)
        {
            StartCoroutine(Shooting());
            agent.stoppingDistance = enemyData.fireRange;
        }
        else
        {
            agent.stoppingDistance = 0;
        }

        recoilForce = 1000 * enemyData.recoilAmount;

        if(enemyData.shotgunEnabled) recoilForce = 250 * enemyData.shotgunBulletAmount;

        /* change the textures and whatnot here



        */
    }

    void Update()
    {
        Rotate(GetAngleToPlayer());
    }
    #region rotation
    private float GetAngleToPlayer()
    {
        Vector2 target = new Vector2(playerTransform.position.x, playerTransform.position.y);
        Vector2 lookDir = target - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        return angle;
    }
    private void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    #endregion

    #region Collisions

    void OnCollisionEnter2D(Collision2D other)
    {
        if(
            other.gameObject.layer == 9 //bullet
        || other.gameObject.layer == 13 //bullet not affected by shield
        || other.gameObject.layer == 16 //exploding bullet
        ) 
        {
            DestroyObject();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 15 || other.gameObject.layer == 19) //15 is explosion layer
        {
            DestroyObject();
        }
    }
    #endregion
    IEnumerator Recoil()
    {
        if(isRecoiling)
        {
            yield break;
        }
        if(enemyData.recoilAmount == 0 && !enemyData.shotgunEnabled)
        {
            yield break;
        }
        WaitForFixedUpdate fixedUpdateWait = new WaitForFixedUpdate();
        isRecoiling = true;
        rb.AddForce(-transform.right * recoilForce);
        yield return fixedUpdateWait;

        // ! Because of a stupid bug I have to detatch the child from the parent temporarily

        transform.SetParent(null, true);

        while(rb.velocity.sqrMagnitude > 1f)
        {
            agent.speed = 0;
            movementTransform.position = transform.position;
            yield return fixedUpdateWait;
        }
        rb.velocity = Vector2.zero;
        movementTransform.position = transform.position;

        // Reattach parent
        transform.SetParent(parentTransform, true);

        agent.speed = enemyData.movementSpeed * 10;
        isRecoiling = false;
    }
    WaitForSeconds frontShieldWaitTime = new WaitForSeconds(.6f);
    IEnumerator FrontShieldToggle()
    {
        if(enemyData.frontShieldEnabled)
        {
            enemyFrontShield.DisableShield();
            yield return frontShieldWaitTime;
            enemyFrontShield.EnableShield();
        }
    }
    IEnumerator Shooting()
    {
        WaitForSeconds fireRate = new WaitForSeconds(enemyData.fireRate);
        while(true)
        {
            if((transform.position - playerTransform.position).magnitude > enemyData.fireRange)
            {
                yield return fireRate;
                break;
            }
            StartCoroutine(FrontShieldToggle());
            StartCoroutine(Recoil());
            SpawnBullet();
            yield return fireRate;
        }
        StartCoroutine(Shooting());
    }
    private void SpawnBullet()
    {
        GameObject obj;
        if(enemyData.explodingBulletsEnabled)
        {
            List<ExplodingBullet> bullets = new List<ExplodingBullet>();
            if(enemyData.shotgunEnabled)
            {
                for (int i = 0; i < enemyData.shotgunBulletAmount - 1; i++)
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
            if(enemyData.shotgunEnabled)
            {
                for (int i = 0; i < enemyData.shotgunBulletAmount - 1; i++)
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
    WaitForSeconds wait = new WaitForSeconds(.1f);
    public void DestroyObject()
    {        
        if(destroyed)
        {
            return;
        }
        destroyed = true;
        
        int random = Random.Range(enemyData.moneyDropMin, enemyData.moneyDropMax);

        while(random >= 10)
        {
            Vector2 randomInsideUnitCircle = Random.insideUnitCircle * .3f;
            random -= 10;
            ObjectPool.SpawnFromPool("Money10", new Vector3(randomInsideUnitCircle.x, randomInsideUnitCircle.y, 0) + transform.position, Quaternion.identity);
        }
        while(random >= 5)
        {
            Vector2 randomInsideUnitCircle = Random.insideUnitCircle * .5f;
            random -= 5;
            ObjectPool.SpawnFromPool("Money5", new Vector3(randomInsideUnitCircle.x, randomInsideUnitCircle.y, 0) + transform.position, Quaternion.identity);
        }
        while(random != 0)
        {
            Vector2 randomInsideUnitCircle = Random.insideUnitCircle * .5f;
            random -= 1;
            ObjectPool.SpawnFromPool("Money1", new Vector3(randomInsideUnitCircle.x, randomInsideUnitCircle.y, 0) + transform.position, Quaternion.identity);
        }

        if(isRecoiling)
        {
            transform.SetParent(parentTransform, true);
        }
        ObjectPool.ReturnToPool("BasicEnemy", transform.parent.gameObject);
    }
}
