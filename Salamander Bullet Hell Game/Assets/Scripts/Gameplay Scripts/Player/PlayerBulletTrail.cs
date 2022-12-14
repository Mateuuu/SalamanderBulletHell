using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletTrail : MonoBehaviour
{
    WaitForSeconds timeIntervalWait;
    Rigidbody2D rb;
    Coroutine spawnBulletCoroutine;
    private bool activated = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetTimeInterval(float interval)
    {
        timeIntervalWait = new WaitForSeconds(interval);
    }
    public void ActivateTrail()
    {
        if(!activated)
        {
            spawnBulletCoroutine = StartCoroutine(SpawnBullets());
        } 
        activated = true;
    }
    public void DeactivateTrail()
    {
        if(activated)
        {
            StopCoroutine(spawnBulletCoroutine);
        } 
        activated = false;
    }
    private IEnumerator SpawnBullets()
    {
        while(true)
        {

            if(rb.velocity.normalized == Vector2.zero)
            {
                yield return timeIntervalWait;
            }
            else
            {
                yield return timeIntervalWait;
                StartCoroutine(SpawnBullet());
            }
        }
    }
    WaitForFixedUpdate fixedUpdateWait = new WaitForFixedUpdate();
    IEnumerator SpawnBullet()
    {
        Vector2 normalizedVelocity = rb.velocity.normalized;
        Vector3 spawnPoint = new Vector3((-normalizedVelocity.x * .4f) + transform.position.x, (-normalizedVelocity.y * .4f) + transform.position.y, 0);
        yield return fixedUpdateWait;
        yield return fixedUpdateWait;
        yield return fixedUpdateWait;
        if(normalizedVelocity == Vector2.zero)
        {
            Debug.Log("test");
            yield break;
        }
        PlayerBullet bullet = ObjectPool.SpawnFromPool("PlayerBullet", spawnPoint, Quaternion.identity).GetComponent<PlayerBullet>();
        bullet.SetSize(0);
        bullet.SetSpeed(0);
    }
}
