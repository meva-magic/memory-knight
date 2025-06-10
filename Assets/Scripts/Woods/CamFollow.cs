using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // Игрок
    public float cameraSmoothness = 0.125f; // Скорость сглаживания

    private void LateUpdate()
    {
        // Текущие координаты камеры
        float camX = transform.position.x;
        float camZ = transform.position.z;

        // Позиция игрока только по оси Y
        float playerY = player.position.y;

        // Желаемая позиция камеры
        Vector3 desiredPosition = new Vector3(camX, playerY, camZ);

        // Плавно двигаемся к новой позиции
        transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSmoothness);
    }
}