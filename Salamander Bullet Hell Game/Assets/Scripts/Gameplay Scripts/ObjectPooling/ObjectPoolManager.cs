using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject explodingBullet;
    [SerializeField] GameObject basicEnemy;
    [SerializeField] GameObject money1;
    [SerializeField] GameObject money5;
    [SerializeField] GameObject money10;
    [SerializeField] GameObject enemySpawnParticles;
    void Awake()
    {
        if(ObjectPool.CheckIfPoolExists("PlayerBullet"))
        {
            ObjectPool.DrainPool("PlayerBullet");
        }
        else
        {
            ObjectPool.NewPool("PlayerBullet", playerBullet);
        }
        ObjectPool.AddObjectsToPool("PlayerBullet", 50);


        if(ObjectPool.CheckIfPoolExists("ExplodingBullet"))
        {
            ObjectPool.DrainPool("ExplodingBullet");
        }
        else
        {
            ObjectPool.NewPool("ExplodingBullet", explodingBullet);
        }
        ObjectPool.AddObjectsToPool("ExplodingBullet", 50);

        
        if(ObjectPool.CheckIfPoolExists("BasicEnemy"))
        {
            ObjectPool.DrainPool("BasicEnemy");
        }
        else
        {
            ObjectPool.NewPool("BasicEnemy", basicEnemy);
        }
        ObjectPool.AddObjectsToPool("BasicEnemy", 20);

        // money
        if(ObjectPool.CheckIfPoolExists("Money1"))
        {
            ObjectPool.DrainPool("Money1");
        }
        else
        {
            ObjectPool.NewPool("Money1", money1);
        }
        ObjectPool.AddObjectsToPool("Money1", 50);

        if(ObjectPool.CheckIfPoolExists("Money5"))
        {
            ObjectPool.DrainPool("Money5");
        }
        else
        {
            ObjectPool.NewPool("Money5", money5);
        }
        ObjectPool.AddObjectsToPool("Money5", 50);

        if(ObjectPool.CheckIfPoolExists("Money10"))
        {
            ObjectPool.DrainPool("Money10");
        }
        else
        {
            ObjectPool.NewPool("Money10", money10);
        }
        ObjectPool.AddObjectsToPool("Money10", 50);
        // end of money

        if(ObjectPool.CheckIfPoolExists("EnemySpawnParticles"))
        {
            ObjectPool.DrainPool("EnemySpawnParticles");
        }
        else
        {
            ObjectPool.NewPool("EnemySpawnParticles", enemySpawnParticles);
        }
        ObjectPool.AddObjectsToPool("EnemySpawnParticles", 10);
    }
}
