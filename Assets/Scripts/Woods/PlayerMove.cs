using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public VariableJoystick joystick;
    public float moveSpeed;
    private Vector2 _joystickInputs;

    private void Update()
    {
        _joystickInputs = joystick.Direction;

        var targetAngle = Mathf.Atan2(_joystickInputs.x, _joystickInputs.y) * Mathf.Rad2Deg;
        var targetRotation = new Vector3(0f, 0f, -targetAngle);
        transform.eulerAngles = targetRotation;
    }

    private void FixedUpdate()
    {
        var targetPosition = ((Vector2)transform.position + _joystickInputs * Time.fixedDeltaTime * moveSpeed);
        transform.position = targetPosition;
    }

    // Новый метод для сброса значений джойстика
    public void ResetJoystickInput()
    {
        _joystickInputs = Vector2.zero;
    }
}