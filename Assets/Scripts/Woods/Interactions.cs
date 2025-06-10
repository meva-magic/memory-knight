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
            playerMoveScript = FindFirstObjectByType<PlayerMove>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            NPC.instance.Interact();
        }
    }

    public void DisablePlayerMovement()
    {
        if (playerMoveScript != null)
            playerMoveScript.enabled = false;
            WoodsUI.instance.DisableJoystick();
    }

    public void EnablePlayerMovement()
    {
        if (playerMoveScript != null)
            playerMoveScript.enabled = true;
            WoodsUI.instance.EnableJoystick();
    }
}
