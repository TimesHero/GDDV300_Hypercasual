using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine.Rendering;
public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; 
    public Transform[] spawnLocations; 
    public float velocity;
    public float secondsTilSpawn;
    public float normalEnemySpawnRate = 0.75f;
    public float hardEnemySpawnRate = 0.25f;
       
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }
       
    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(secondsTilSpawn);
        SpawnObstacle();
        StartCoroutine(SpawnTimer());
    }
    private void SpawnObstacle()
    {   
        float[] spawnWeights = new float[] {normalEnemySpawnRate, hardEnemySpawnRate};
        float totalWeight = spawnWeights.Sum();
        float randomValue = Random.Range(0f, totalWeight);
        int enemyType = (randomValue < spawnWeights[0]) ? 0 : 1; 
            
        int whereSpawn = Random.Range(0,spawnLocations.Length);
        Transform spawnPoint = spawnLocations[whereSpawn];
        GameObject enemy = Instantiate(enemyPrefabs[enemyType], spawnPoint.position, spawnPoint.rotation);
        Vector3 spawnDirection;
        if (spawnPoint.position.x > 0f)
            spawnDirection = Vector3.left;
        else
            spawnDirection = Vector3.right; 
                
        enemy.GetComponent<EnemyHandler>().SetVelocity(spawnDirection*velocity);
    }
    

}
