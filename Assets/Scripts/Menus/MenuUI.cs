using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public static MenuUI instance;

    [SerializeField] private Button startButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startButton.onClick.AddListener(LoadGame);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
