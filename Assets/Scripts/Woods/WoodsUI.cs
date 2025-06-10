using UnityEngine;

public class WoodsUI : MonoBehaviour
{
    public GameObject joystick;
    public GameObject canvasParent; // Родительский объект для управления UI

    public static WoodsUI instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EnableJoystick();
    }

    public void EnableJoystick()
    {
        joystick.SetActive(true);
    }

    public void DisableJoystick()
    {
        joystick.SetActive(false);
    }

    /// <summary>
    /// Поиск элементов UI по тегу "UI" и скрытие их
    /// </summary>
    public void DisableAllUI()
    {
        // Получаем список всех объектов с тегом "UI"
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag("UI");

        foreach(GameObject element in uiElements)
        {
            // Деактивируем только элементы UI, принадлежащие родительскому канвасу
            if(element.transform.IsChildOf(canvasParent.transform))
            {
                element.SetActive(false);
            }
        }
    }
}