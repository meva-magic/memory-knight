using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    public string Theme;

    private void Start()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.Play(Theme);

        if (startButton != null)
            startButton.onClick.AddListener(() => SceneManager.LoadScene("KnightStart"));
        
        if (restartButton != null)
            restartButton.onClick.AddListener(() => SceneManager.LoadScene("Leshy"));
        
        if (menuButton != null)
            menuButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        
        if (settingsButton != null)
            settingsButton.onClick.AddListener(() => SceneManager.LoadScene("Settings"));
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
