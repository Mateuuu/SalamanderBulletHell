using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [HideInInspector] public float shieldSlowdown = .1f;
    [HideInInspector] public float bounceProbability = 1f;
    [HideInInspector] public bool shieldDeflectEnabled = false;
    public void SetShieldRadius(float radius)
    {
        transform.localScale = new Vector3(radius, radius, 1f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 9 || other.gameObject.layer == 16) //9 is bullet layer
        {
            IBullet bullet = other.gameObject.GetComponent<IBullet>();
            if(shieldDeflectEnabled)
            {
                if(Random.Range(0f,  1f) < bounceProbability)
                {
                    float angle = GetAngleToPlayer(new Vector2(other.transform.position.x, other.transform.position.y));
                    Vector2 direction = new Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle));

                    Vector3 newVelocity = direction.normalized * bullet.GetVelocityMagnitude();

                    if(Random.Range(0f, 1f) > .05f)
                    {
                        bullet.SetVelocity(-newVelocity);
                    }
                    else
                    {
                        bullet.SetVelocity(newVelocity * .2f);
                    }
                }
                else
                {
                    bullet.SetVelocity(bullet.GetVelocity() * shieldSlowdown);
                }
            }
            else
            {
                bullet.SetVelocity(bullet.GetVelocity() * shieldSlowdown);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        IBullet bullet = other.gameObject.GetComponent<IBullet>();

        if(bullet != null)
        {
            if(other.gameObject.layer == 13)
            {
                bullet.SetCorrectLayer();
                return;
            }
            bullet.SetDefaultVelocity();
        }
    }
    private float GetAngleToPlayer(Vector2 position)
    {
        Vector2 lookDir = new Vector2(transform.position.x, transform.position.y) - position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);
        return angle;
    }
}
