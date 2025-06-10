using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    [Header("UI References")]
    public Text scoreText;
    
    [Header("Score Settings")]
    public int currentScore;
    public int maxScore { get; private set; }

    public int Score => currentScore;

    void Awake()
    {
        if (scoreText == null)
        {
            scoreText = GetComponent<Text>();
            Debug.LogWarning("ScoreText reference not set, getting from component");
        }
    }

    public void IncrementScore()
    {
        currentScore++;
        UpdateDisplay();
    }

    public void ResetScore(int maxPossibleScore)
    {
        currentScore = 0;
        maxScore = maxPossibleScore;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Pairs: {currentScore}/{maxScore}";
        }
    }
}
