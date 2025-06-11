using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    [Header("Scene Names")]
    public string winScene = "WinScene";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent if needed
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadEndScene(bool isWin)
    {
        if (isWin)
        {
            // Only load win scene if player wins
            DestroyAllPersistentUI();
            SceneManager.LoadScene(winScene, LoadSceneMode.Single);
        }
        // No else case here - lose condition is now handled by DuelManager showing restart panel
    }

    public void ReloadCurrentScene()
    {
        DestroyAllPersistentUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void LoadMenuScene()
    {
        DestroyAllPersistentUI();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void DestroyAllPersistentUI()
    {
        // Find all active and inactive canvases
        Canvas[] allCanvases = FindObjectsOfType<Canvas>(true);

        foreach (Canvas canvas in allCanvases)
        {
            // If the canvas is in the "DontDestroyOnLoad" scene, destroy it
            if (canvas.gameObject.scene.name == "DontDestroyOnLoad")
            {
                Destroy(canvas.gameObject);
            }
        }
    }
}