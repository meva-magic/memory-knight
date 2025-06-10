using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public static MenuUI instance;

    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startButton.onClick.AddListener(LoadGame);
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(Menu);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Start");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Woods");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
