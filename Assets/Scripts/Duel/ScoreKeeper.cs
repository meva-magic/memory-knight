using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoreText;
    private int currentScore;
    private int maxScore;

    // Публичное свойство для получения текущего балла
    public int Score => currentScore;

    public void IncrementScore()
    {
        currentScore++;
        scoreText.text = $"Score: {currentScore}/{maxScore}";
        if (currentScore == maxScore)
        {
            //FindObjectOfType<DuelManager>().EndGame();
        }
    }

    public void ResetScore(int maxPossibleScore)
    {
        currentScore = 0;
        maxScore = maxPossibleScore;
        scoreText.text = $"Score: {currentScore}/{maxScore}";
    }
}