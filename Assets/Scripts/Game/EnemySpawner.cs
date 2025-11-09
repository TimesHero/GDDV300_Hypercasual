using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs; 
        public Transform[] spawnLocations; 
        public float velocity; 
        public float secondsTilSpawn;
       
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
            int enemyType = Random.Range(0,enemyPrefabs.Length);
            int whereSpawn = Random.Range(0,spawnLocations.Length);
            Transform spawnPoint = spawnLocations[whereSpawn];
            GameObject enemy = Instantiate(enemyPrefabs[enemyType], spawnPoint.position, spawnPoint.rotation);
            Vector3 spawnDirection;
            if (spawnPoint.position.x > 0f)
                spawnDirection = Vector3.left;
            else
                spawnDirection = Vector3.right; 
                
            enemy.GetComponent<VelocitySetter>().SetVelocity(spawnDirection*velocity);
        }
    

    }
