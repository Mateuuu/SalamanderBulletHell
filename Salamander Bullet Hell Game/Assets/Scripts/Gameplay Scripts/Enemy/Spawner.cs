using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] WaveIndicator waveIndicator;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] public Wave[] waves;
    int wave = 0; //0 is starting wave
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);

        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        waveIndicator.ExecuteWave(waves[wave].duration);
        List<EnemyData> enemiesToSpawn = new List<EnemyData>();

        foreach(EnemyWaveInfo enemyWaveInfo in waves[wave].enemiesInWave)
        {
            for (int i = 0; i < enemyWaveInfo.enemyAmount; i++)
            {
                enemiesToSpawn.Add(enemyWaveInfo.enemyData);
            }
        }

        while(enemiesToSpawn.Count > 0)
        {
            Transform randomPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject particle = ObjectPool.SpawnFromPool("EnemySpawnParticles", randomPosition.position, Quaternion.identity);
            particle.GetComponent<ParticleSystem>().startLifetime = waves[wave].spawnInterval;

            yield return new WaitForSeconds(waves[wave].spawnInterval);

            GameObject obj = ObjectPool.SpawnFromPool("BasicEnemy", randomPosition.position, Quaternion.identity);
            Enemy enemy = obj.GetComponentInChildren<Enemy>();

            int random = Random.Range(0, enemiesToSpawn.Count);

            enemy.enemyData = enemiesToSpawn[random];
            enemiesToSpawn.RemoveAt(random);

            enemy.ResetData();
        }

        wave++;
        waveIndicator.NewWave();
        yield return new WaitForSeconds(5f);
        if(wave < 25)
        {
            StartCoroutine(Spawning());
        }

    }
}
[System.Serializable]
public class Wave
{
    public float duration;
    public float spawnInterval => duration / GetNumOfEnemies();
    public List<EnemyWaveInfo> enemiesInWave;

    public int GetNumOfEnemies()
    {
        int numOfEnemies = 0;
        for (int i = 0; i < enemiesInWave.Count; i++)
        {
            numOfEnemies += enemiesInWave[i].enemyAmount;
        }
        return numOfEnemies;
    }
}

[System.Serializable]
public class EnemyWaveInfo
{
    public EnemyData enemyData;
    public int enemyAmount;
}