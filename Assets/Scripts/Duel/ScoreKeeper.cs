using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoreText;
    private int _currentScore;
    private int maxScore;

    public int Score => _currentScore; // Публичное свойство Score

    public void IncrementScore()
    {
        _currentScore++; // инкрементируем внутренний счётчик
        scoreText.text = $"Score: {_currentScore}/{maxScore}";
        if (_currentScore == maxScore)
        {
            FindFirstObjectByType<DuelManager>().EndGame();
        }
    }

    public void ResetScore(int maxPossibleScore)
    {
        _currentScore = 0;
        maxScore = maxPossibleScore;
        scoreText.text = $"Score: {_currentScore}/{maxScore}";
    }
}