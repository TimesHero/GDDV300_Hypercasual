using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform target;        
    public Transform mover;       
    public Transform pointA;        
    public Transform pointB;        

    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float returnSpeed = 2f;
    public float returnDelay = 0.5f;
    [Header("Health Settings")]
    public float hungerLevel = 10f;
    public float hungerDepleationRate = 0.005f;
    public Slider hungerMeter;
    [Header("Score Settings")]
    public int score;
    public TextMeshProUGUI scoreText;
    [Header("Game Over Settings")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI endScore;
    public TextMeshProUGUI endCurrency;
    public TextMeshProUGUI newHighScore;
    public bool isPoisoned;
    private float poisonSpeed = 0;
    public float poisonValue = 0.5f;
    private bool isHolding = false;
    private bool isReturning = false;
    private float t = 0f;      
    private int direction = 1;     

    private Vector3 targetStartPos;
    private Vector3 moverStartPos;
    private GameObject caughtEnemy = null;
    public bool bossHit = false;
    private int ateWhilePoisoned = 0;
    public EnemySpawner enemySpawner;
    void Start()
    {
        targetStartPos = target.position;
        moverStartPos = mover.position;
    }

    void Update()
    {
        if (Time.timeScale==1)
        {
            hungerMeter.value = hungerLevel;
            hungerLevel -= hungerDepleationRate;
        }
        if (hungerLevel <=0)
        {
            if (!gameOverPanel.activeInHierarchy)
            GameOver();    
        }
        if (isReturning) return; 

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isHolding = true;
        }
        if (Input.GetMouseButtonUp(0)&&isHolding)
        {
            isHolding = false;
            mover.position = target.position;  
            StartCoroutine(ReturnToStart());
        }

        if (isHolding)
        {
            MoveTarget();
        }

        
    }

    void MoveTarget()
    {
        t += direction * moveSpeed * Time.deltaTime;
        if (isPoisoned)
            t += direction * moveSpeed*  -0.5f * Time.deltaTime;
        
        if (t > 1f)
        {
            t = 1f;
            direction = -1;
        }
        else if (t < 0f)
        {
            t = 0f;
            direction = 1;
        }

        target.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }

    IEnumerator ReturnToStart()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);

        Vector3 targetInitial = target.position;
        Vector3 moverInitial = mover.position;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            if (isPoisoned)
            {
                if (caughtEnemy != null)
                    elapsed += Time.deltaTime * returnSpeed * 0.5f;
                else
                    elapsed += Time.deltaTime * returnSpeed * 0.5f / 3;
            }
            else
            {
                if (caughtEnemy != null)
                    elapsed += Time.deltaTime * returnSpeed;
                else
                    elapsed += Time.deltaTime * returnSpeed / 3;
            }
                
            float lerpT = Mathf.Clamp01(elapsed);

            target.position = Vector3.Lerp(targetInitial, targetStartPos, lerpT);
            mover.position = Vector3.Lerp(moverInitial, moverStartPos, lerpT);

            yield return null;
        }
        if (caughtEnemy != null)
        {
            score += caughtEnemy.GetComponent<EnemyHandler>().scoreValue;
            scoreText.text = $"Score: {score}";
            Destroy(caughtEnemy);
            caughtEnemy = null;
            hungerLevel += 2.5f;
            enemySpawner.enemiesKilled++;
            if (hungerLevel>10)
                hungerLevel=10f;
            ateWhilePoisoned++;
            if (ateWhilePoisoned==4)
            {
                isPoisoned=false;
                ateWhilePoisoned=0;
            }
        }
        else if (bossHit)
        {
            hungerLevel = 10f;
        }
        t = 0f;
        direction = 1;
        isReturning = false;
        bossHit = false;

    
    }
    public void GameOver()
    {
        Time.timeScale=0f;
        gameOverPanel.SetActive(true);
        endScore.text = $"Score: {score}";
        int currencyEarned = score / 10;
        endCurrency.text = $"Soft Currency Earned: {currencyEarned}";
        List<int> topScores = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            int savedScore = PlayerPrefs.GetInt("HighScore" + i, 0);
            topScores.Add(savedScore);
            Debug.Log($"Loaded score {i}: {savedScore}");
        }

        topScores.Add(score);
        Debug.Log($"Added current score: {score}");
        topScores.Sort((a, b) => b.CompareTo(a)); 
        topScores = topScores.Take(5).ToList(); 

        for (int i = 0; i < topScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, topScores[i]);
            Debug.Log($"Saved top score {i}: {topScores[i]}");
        }

        if (score >= topScores[0])
        {
            newHighScore.text = "New high score!";
            Debug.Log("New high score achieved!");
        }
        else
        {
            newHighScore.text = "";
        }

        int currentCurrency = PlayerPrefs.GetInt("SoftCurrency", 0); 
        currentCurrency += currencyEarned;
        PlayerPrefs.SetInt("SoftCurrency", currentCurrency);
            PlayerPrefs.Save();

        //DEBUG TESTING
        Debug.Log("Final Top 5 Scores:");
        for (int i = 0; i < topScores.Count; i++)
        {
            Debug.Log($"{i + 1}: {topScores[i]}");
        }
}


   public void OnTongueHit2D(Collider2D other)
    {
        if (caughtEnemy == null && other.CompareTag("Enemy"))
        {
            caughtEnemy = other.gameObject;
            caughtEnemy.transform.SetParent(mover);
            if (caughtEnemy.GetComponent<EnemyHandler>().applyPoison)
            {
                isPoisoned = true;
            }
            Rigidbody2D rb = caughtEnemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;    
                rb.isKinematic = true;        
            }
            EnemyHandler vs = caughtEnemy.GetComponent<EnemyHandler>();
            if (vs != null)
            {
                vs.enabled = false;
            }
        }
    }
}
