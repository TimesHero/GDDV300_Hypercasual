using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Slider progressionSlider;

    public int scoreThreshold = 2000;
    public int playerScore;

    public Transform[] spawnLocations;
    public Transform bossSpawn;

    public float velocity;
    public float secondsTilSpawn;

    public float waveSpawnRateModifier = 1f;

    private bool bossWave;
    public int currentWave = 1;

    private int poisonIndex = 2;
    private float[] spawnWeights;

    public TextMeshProUGUI wavePopupText;
    public GameObject warningObject;  

    private List<GameObject> currentEnemies = new List<GameObject>(); 

    void Start()
    {
        spawnWeights = new float[] { 0.75f, 0.25f };
        StartCoroutine(SpawnTimer());
        ShowWavePopup(currentWave);
    }

    void Update()
    {
        if (!bossWave)
            progressionSlider.value = playerScore;

        if (playerScore >= scoreThreshold)
        {
            playerScore = 0;
            bossWave = true;
            StartCoroutine(StartWarningSequence()); // Start warning sequence before boss spawn
        }
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(secondsTilSpawn);

        if (!bossWave)
            SpawnObstacle();

        StartCoroutine(SpawnTimer());
    }

    public void DefeatedBoss()
    {
        currentWave++;
        progressionSlider.value = 0;
        bossWave = false;
        velocity += 0.5f;
        secondsTilSpawn += 0.5f;
        ShowWavePopup(currentWave);
    }

    private void ShowWavePopup(int waveNumber)
    {
        if (wavePopupText != null)
        {
            if (waveNumber>=2)
                wavePopupText.text = "Wave " + waveNumber;
            wavePopupText.gameObject.SetActive(true);
            StartCoroutine(HidePopupAfterDelay(2f));
        }
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        wavePopupText.gameObject.SetActive(false);
    }

    private void SpawnObstacle()
    {
        List<int> enemyTypes = new List<int>();
        List<float> weights = new List<float>();

        enemyTypes.Add(0);
        weights.Add(1.0f);

        if (currentWave >= 2 && enemyPrefabs.Length > 1)
        {
            enemyTypes.Add(1);
            weights.Add(0.5f);
        }
        if (currentWave >= 3 && enemyPrefabs.Length > poisonIndex)
        {
            enemyTypes.Add(poisonIndex);
            weights.Add(0.2f);
        }

        float total = weights.Sum();
        float r = Random.value * total;

        float running = 0f;
        int selectedEnemy = enemyTypes[0];

        for (int i = 0; i < weights.Count; i++)
        {
            running += weights[i];
            if (r <= running)
            {
                selectedEnemy = enemyTypes[i];
                break;
            }
        }

        int spawnIndex = Random.Range(0, spawnLocations.Length);
        Transform spawnPoint = spawnLocations[spawnIndex];

        GameObject enemy = Instantiate(
            enemyPrefabs[selectedEnemy],
            spawnPoint.position,
            spawnPoint.rotation
        );

        currentEnemies.Add(enemy); 

        Vector3 spawnDirection = spawnPoint.position.x > 0f ? Vector3.left : Vector3.right;

        var handler = enemy.GetComponent<EnemyHandler>();
        handler.SetVelocity(spawnDirection * velocity);
    }

    private IEnumerator StartWarningSequence()
    {
        MoveEnemiesOffScreen();
        if (warningObject != null)
        {
            warningObject.SetActive(true);
            yield return new WaitForSeconds(2f); 
            warningObject.SetActive(false);
        }
        Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation);
    }

    private void MoveEnemiesOffScreen()
{
    // Find all objects tagged with "Enemy"
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    // Loop through each enemy and call the Leave() method
    foreach (GameObject enemy in enemies)
    {
        // Ensure the enemy has an EnemyHandler component before calling Leave()
        var enemyHandler = enemy.GetComponent<EnemyHandler>();
        if (enemyHandler != null)
        {
            enemyHandler.Leave(); // Call the Leave method to move the enemy off-screen
        }
    }
}

}
