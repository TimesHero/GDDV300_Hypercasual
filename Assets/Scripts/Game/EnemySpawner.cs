using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public GameObject bossPrefab;
    public int enemiesPerWave = 20;
    public int enemiesKilled; 
    public Transform[] spawnLocations; 
    public Transform bossSpawn;
    public float velocity;
    public float secondsTilSpawn;
    public float normalEnemySpawnRate = 0.75f;
    public float hardEnemySpawnRate = 0.25f;
    private bool bossWave;

    private float[] spawnWeights;  // Cached spawn weights array

    void Start()
    {
        spawnWeights = new float[] { normalEnemySpawnRate, hardEnemySpawnRate }; // Cache spawn weights
        StartCoroutine(SpawnTimer());
    }

    void Update()
    {
        if (enemiesKilled == enemiesPerWave)
        {
            enemiesKilled = 0;
            bossWave = true;
            Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation);
        }
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(secondsTilSpawn);
        if (!bossWave)
        {
            SpawnObstacle();
        }
        StartCoroutine(SpawnTimer());
    }

    public void DefeatedBoss()
    {
        velocity += 0.5f;
        secondsTilSpawn += 1f;
        bossWave = false;
    }

    private void SpawnObstacle()
    {
        float totalWeight = spawnWeights.Sum();
        float randomValue = Random.Range(0f, totalWeight);
     
        int enemyType = (randomValue < spawnWeights[0]) ? 0 : 1;

        int whereSpawn = Random.Range(0, spawnLocations.Length);
        Transform spawnPoint = spawnLocations[whereSpawn];


        GameObject enemy = null;
        int isPoisonType = Random.Range(0, 8);
        if (isPoisonType == 2) 
        {
            enemy = Instantiate(enemyPrefabs[isPoisonType], spawnPoint.position, spawnPoint.rotation);
        }
        else  
        {
            enemy = Instantiate(enemyPrefabs[enemyType], spawnPoint.position, spawnPoint.rotation);
        }


        Vector3 spawnDirection = spawnPoint.position.x > 0f ? Vector3.left : Vector3.right;

        var enemyHandler = enemy.GetComponent<EnemyHandler>();
        enemyHandler.SetVelocity(spawnDirection * velocity);
    }
}
