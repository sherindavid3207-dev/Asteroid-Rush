using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private Vector2 moveBounds = new Vector2(8f, 4.5f);

    [Header("Cockpit Banking Visuals")]
    [SerializeField] private float bankAmount = 12f;
    [SerializeField] private float smoothTime = 8f;

    private void Update()
    {
        float inputX = 0f;
        float inputY = 0f;

        // 1. Read Keyboard Input (WASD + Arrow Keys)
        if (Keyboard.current != null)
        {
            var kbd = Keyboard.current;

            // Horizontal (A/D or Left/Right)
            if (kbd.aKey.isPressed || kbd.leftArrowKey.isPressed) inputX -= 1f;
            if (kbd.dKey.isPressed || kbd.rightArrowKey.isPressed) inputX += 1f;

            // Vertical (W/S or Up/Down)
            if (kbd.wKey.isPressed || kbd.upArrowKey.isPressed) inputY += 1f;
            if (kbd.sKey.isPressed || kbd.downArrowKey.isPressed) inputY -= 1f;
        }

        // 2. Read Gamepad Stick Input (Optional controller support)
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();
            if (Mathf.Abs(stick.x) > 0.1f) inputX = stick.x;
            if (Mathf.Abs(stick.y) > 0.1f) inputY = stick.y;
        }

        // Normalize directional input vector to prevent diagonal speed boosting
        Vector2 inputVector = Vector2.ClampMagnitude(new Vector2(inputX, inputY), 1f);

        // 3. Move ship position
        Vector3 moveDirection = new Vector3(inputVector.x, inputVector.y, 0f) * (moveSpeed * Time.deltaTime);
        transform.Translate(moveDirection, Space.World);

        // 4. Clamp position within screen boundaries
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, -moveBounds.x, moveBounds.x);
        currentPos.y = Mathf.Clamp(currentPos.y, -moveBounds.y, moveBounds.y);
        transform.position = currentPos;

        // 5. Apply subtle cockpit banking rotation when steering
        Quaternion targetRotation = Quaternion.Euler(-inputVector.y * bankAmount, 0f, -inputVector.x * bankAmount);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothTime);
    }
}