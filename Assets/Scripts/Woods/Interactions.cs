using UnityEngine;

public class Interaction : MonoBehaviour
{
    public PlayerMove playerMoveScript;

    public static Interaction instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!playerMoveScript)
        {
            playerMoveScript = FindObjectOfType<PlayerMove>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что столкнулись именно с NPC
        if (other.CompareTag("NPC"))
        {
            NPC.instance.Interact();
            other.gameObject.SetActive(false); // отключаем триггер, чтобы предотвратить повторный запуск
        }
    }

    public void DisablePlayerMovement()
    {
        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = false;
            WoodsUI.instance.DisableJoystick();
        }
    }

    public void EnablePlayerMovement()
    {
        if (playerMoveScript != null)
        {
            playerMoveScript.ResetJoystickInput(); // очистка предыдущего направления движения
            playerMoveScript.enabled = true;
            WoodsUI.instance.EnableJoystick();
        }
    }
}