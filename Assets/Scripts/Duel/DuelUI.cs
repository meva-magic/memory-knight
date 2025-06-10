using UnityEngine;
using UnityEngine.SceneManagement;

public class DuelUI : MonoBehaviour
{
    [Header("References")]
    public ScoreKeeper scoreKeeper;
    public TimerController timerController;
    public string winSceneName = "WoodsWin";
    public string loseSceneName = "WoodsLose";

    private DuelManager duelManager;
    private bool gameEnded = false;

    void Awake()
    {
        duelManager = GetComponent<DuelManager>();
        if (duelManager == null)
        {
            duelManager = FindObjectOfType<DuelManager>();
            Debug.LogWarning("DuelManager reference not set, finding in scene");
        }

        if (scoreKeeper == null)
        {
            scoreKeeper = FindObjectOfType<ScoreKeeper>();
            Debug.LogWarning("ScoreKeeper reference not set, finding in scene");
        }

        if (timerController == null)
        {
            timerController = FindObjectOfType<TimerController>();
            Debug.LogWarning("TimerController reference not set, finding in scene");
        }
    }

    public void InitializeGame()
    {
        if (duelManager == null || scoreKeeper == null || timerController == null)
        {
            Debug.LogError("Missing critical components!");
            enabled = false;
            return;
        }

        gameEnded = false;
        int totalPairs = duelManager.GetTotalPairs();
        scoreKeeper.ResetScore(totalPairs);
        timerController.ResetTimer(duelManager.gameDuration);
    }

    public void OnMatchFound()
    {
        if (gameEnded || scoreKeeper == null) return;
        
        scoreKeeper.IncrementScore();
        
        if (scoreKeeper.Score >= scoreKeeper.maxScore)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        gameEnded = true;
        timerController?.StopTimer();
        SceneManager.LoadScene(winSceneName);
    }

    public void OnTimeExpired()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            SceneManager.LoadScene(loseSceneName);
        }
    }
}
