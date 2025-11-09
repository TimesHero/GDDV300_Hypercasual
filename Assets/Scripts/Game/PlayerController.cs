using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

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
    public int frogHealth = 3;
    public GameObject[] healthIcons;
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
    private bool isHolding = false;
    private bool isReturning = false;
    private float t = 0f;      
    private int direction = 1;     

    private Vector3 targetStartPos;
    private Vector3 moverStartPos;
    private GameObject caughtEnemy = null;

    void Start()
    {
        targetStartPos = target.position;
        moverStartPos = mover.position;
    }

    void Update()
    {
        if (isReturning) return; 

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isHolding = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
            mover.position = target.position;  
            StartCoroutine(ReturnToStart());
        }

        if (isHolding)
        {
            MoveTarget();
        }
        if (frogHealth != 0)
        {
            hungerMeter.value = hungerLevel;
            hungerLevel -= hungerDepleationRate;
            if (hungerLevel < 1)
            {
                hungerLevel = 10f;
                healthIcons[frogHealth - 1].SetActive(false);
                frogHealth--;
                if (frogHealth==0)
                {
                    GameOver();
                    Time.timeScale = 0;
                }
            }
        }
    }

    void MoveTarget()
    {
        t += direction * moveSpeed * Time.deltaTime;
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
            elapsed += Time.deltaTime * returnSpeed;
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
            hungerLevel = 10f;
        }
        else
        {
            healthIcons[frogHealth - 1].SetActive(false);
            frogHealth--;
            if (frogHealth == 0)
            {
                GameOver();
                Time.timeScale = 0;
            }
        }
        t = 0f;
        direction = 1;
        isReturning = false;
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        endScore.text = $"Score: {score}";
        int currencyEarned = score / 10;
        endCurrency.text = $"Soft Currency Eanred: {currencyEarned}";
        if (score > PlayerPrefs.GetInt("HighScore", 0)) 
        {
            PlayerPrefs.SetInt("HighScore", score);
            newHighScore.text = "New high score!";
        }

        int currentCurrency = PlayerPrefs.GetInt("SoftCurrency", 0); 
        currentCurrency += currencyEarned;
        PlayerPrefs.SetInt("SoftCurrency", currentCurrency); 
        PlayerPrefs.Save();
    }
   public void OnTongueHit2D(Collider2D other)
    {
        if (caughtEnemy == null && other.CompareTag("Enemy"))
        {
            caughtEnemy = other.gameObject;
            caughtEnemy.transform.SetParent(mover);
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
