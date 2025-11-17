using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Leaderboardmanager: MonoBehaviour
{
    [Header("UI References")]
    public GameObject entryPrefab;      
    public Transform entryContainer;  

    private const int MaxEntries = 5;

    void Start()
    {
        PopulateLeaderboard();
    }

    void PopulateLeaderboard()
    {
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }

        List<int> scores = new List<int>();
        for (int i = 0; i < MaxEntries; i++)
        {
            int s = PlayerPrefs.GetInt("HighScore" + i, 0);
            scores.Add(s);
        }

        scores.Sort((a, b) => b.CompareTo(a));

        for (int i = 0; i < MaxEntries; i++)
        {
            GameObject newEntry = Instantiate(entryPrefab, entryContainer);

            TMP_Text[] tmpTexts = newEntry.GetComponentsInChildren<TMP_Text>();

            TMP_Text rankText = tmpTexts[0];
            TMP_Text scoreText = tmpTexts[1];

            rankText.text = (i + 1).ToString() + ".";  
            scoreText.text = scores[i].ToString();
        }
    }
}