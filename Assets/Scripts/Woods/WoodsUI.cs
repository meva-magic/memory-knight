using UnityEngine;

public class WoodsUI : MonoBehaviour
{
    public GameObject joystick;

    public static WoodsUI instance;
    public string Theme;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.Play(Theme);
        EnableJoystick();
    }

    public void EnableJoystick()
    {
        DisableAllUI();
        joystick.SetActive(true);
    }

    public void DisableJoystick()
    {
        DisableAllUI();
        joystick.SetActive(false);
    }

    public void DisableAllUI()
    {
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag("UI");

        foreach(GameObject element in uiElements)
        {
            element.SetActive(false);
        }
    }
}
