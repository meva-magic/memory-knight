using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("Scene Names")]
    public string winScene = "WinScene";
    public string loseScene = "LoseScene";

    private void Awake()
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

    public void LoadEndScene(bool isWin)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(isWin ? winScene : loseScene);
    }
}
