using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;

    public ScoreEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    [Header("Interface References")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private int maxEntries = 5;
    [SerializeField] private PlayerController playerController;

    // Backing list used by SortScores and RefreshUI
    private List<ScoreEntry> scores = new List<ScoreEntry>();

    // Load scores from PlayerPrefs (written by PlayerController) into the list
    private void LoadScoresFromPrefs()
    {
        scores.Clear();

        for (int i = 0; i < maxEntries; i++)
        {
            int savedScore = PlayerPrefs.GetInt("HighScore" + i, 0);

            if (savedScore > 0)
            {
                // No names are stored, so this is just a placeholder
                string playerName = "Player " + (i + 1);
                scores.Add(new ScoreEntry(playerName, savedScore));
            }
        }

        SortScores();
    }

    // Optional: add a score at runtime (if you ever need it)
    public void AddScore(string playerName, int scoreValue)
    {
        scores.Add(new ScoreEntry(playerName, scoreValue));
        SortScores();
        RefreshUI();
    }

    public void ShowLeaderboard()
    {
        LoadScoresFromPrefs();
        RefreshUI();

        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(true);
        }
    }

    public void HideLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
        }
    }

    private void SortScores()
    {
        scores.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private void RefreshUI()
    {
        if (entryContainer == null || entryPrefab == null)
            return;

        // Clear existing rows
        for (int i = entryContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(entryContainer.GetChild(i).gameObject);
        }

        int count = Mathf.Min(scores.Count, maxEntries);

        for (int i = 0; i < count; i++)
        {
            ScoreEntry entry = scores[i];

            GameObject row = Instantiate(entryPrefab, entryContainer);

            // Assumes the prefab has at least three TMP_Text components
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 3)
            {
                // 0: rank, 1: name, 2: score
                texts[0].text = (i + 1).ToString() + ".";
                texts[1].text = entry.playerName;
                texts[2].text = entry.score.ToString();
            }
        }
    }
}
