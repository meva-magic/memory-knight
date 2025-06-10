using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("Scene Names")]
    public string winScene = "WinScene";
    public string loseScene = "LoseScene";
    public string mainMenu = "MainMenu";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndDuel(bool isWin)
    {
        DuelManager.Instance.ForceEndDuel();
        SceneManager.LoadScene(isWin ? winScene : loseScene);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
