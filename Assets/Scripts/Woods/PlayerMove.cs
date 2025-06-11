using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("References")]
    public VariableJoystick joystick;
    
    [Header("Settings")]
    public float moveSpeed = 5f;

    private Vector2 _joystickInputs;
    private bool _isMovementEnabled = true;

    private void Update()
    {
        if (!_isMovementEnabled)
        {
            _joystickInputs = Vector2.zero;
            return;
        }

        // Get joystick input
        _joystickInputs = joystick.Direction;

        // Rotate player based on input direction
        if (_joystickInputs.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(_joystickInputs.x, _joystickInputs.y) * Mathf.Rad2Deg;
            Vector3 targetRotation = new Vector3(0f, 0f, -targetAngle);
            transform.eulerAngles = targetRotation;
        }
    }

    private void FixedUpdate()
    {
        if (!_isMovementEnabled) return;

        // Apply movement
        Vector2 targetPosition = (Vector2)transform.position + _joystickInputs * (Time.fixedDeltaTime * moveSpeed);
        transform.position = targetPosition;
    }

    public void DisableMovement()
    {
        _isMovementEnabled = false;
        _joystickInputs = Vector2.zero;
    }

    public void EnableMovement()
    {
        _isMovementEnabled = true;
        _joystickInputs = Vector2.zero;
        
        // Force reset joystick position if available
        if (joystick != null)
        {
            joystick.OnPointerUp(null); // Simulates releasing the joystick
        }
    }
}
