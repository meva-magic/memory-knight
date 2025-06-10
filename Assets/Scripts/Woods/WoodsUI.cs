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

    public void DisableAllUI()
    {
        if (canvasParent != null)
        {
            for(int i = 0; i < canvasParent.transform.childCount; i++)
            {
                Transform childTransform = canvasParent.transform.GetChild(i);
                childTransform.gameObject.SetActive(false); // Деактивируем каждый дочерний объект отдельно
            }
        }
    }
}