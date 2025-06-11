using UnityEngine;
using UnityEngine.SceneManagement;

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
        }

        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadEndScene(bool isWin)
    {
        // Destroy all persistent UI elements (if any)
        DestroyAllPersistentUI();

        // Load the new scene in Single mode (unloads previous scene)
        SceneManager.LoadScene(
            isWin ? winScene : loseScene,
            LoadSceneMode.Single
        );
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
