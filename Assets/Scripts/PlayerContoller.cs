using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Forward Speed (Constant)")]
    [SerializeField] private float forwardSpeed = 25f;

    [Header("Steering Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private Vector2 moveBounds = new Vector2(8f, 4.5f);

    [Header("Cockpit Banking Visuals")]
    [SerializeField] private float bankAmount = 12f;
    [SerializeField] private float smoothTime = 8f;

    [Header("Speed & Engine Shake Effect")]
    [SerializeField] private float engineShakeAmount = 0.015f;
    [SerializeField] private Transform cockpitCameraTransform;

    private Vector3 _originalCamPos;

    private void Start()
    {
        if (cockpitCameraTransform != null)
        {
            _originalCamPos = cockpitCameraTransform.localPosition;
        }
    }

    private void Update()
    {
        // 1. CONSTANT AUTOMATIC FORWARD MOVEMENT (No button needed!)
        transform.Translate(Vector3.forward * (forwardSpeed * Time.deltaTime), Space.World);

        // 2. Read Steering Input (WASD / Arrows)
        float inputX = 0f;
        float inputY = 0f;

        if (Keyboard.current != null)
        {
            var kbd = Keyboard.current;
            if (kbd.aKey.isPressed || kbd.leftArrowKey.isPressed) inputX -= 1f;
            if (kbd.dKey.isPressed || kbd.rightArrowKey.isPressed) inputX += 1f;
            if (kbd.wKey.isPressed || kbd.upArrowKey.isPressed) inputY += 1f;
            if (kbd.sKey.isPressed || kbd.downArrowKey.isPressed) inputY -= 1f;
        }

        Vector2 inputVector = Vector2.ClampMagnitude(new Vector2(inputX, inputY), 1f);

        // 3. Move ship laterally (X and Y axis) while moving forward
        Vector3 lateralMove = new Vector3(inputVector.x, inputVector.y, 0f) * (moveSpeed * Time.deltaTime);
        transform.Translate(lateralMove, Space.World);

        // 4. Clamp lateral position relative to current forward movement
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, -moveBounds.x, moveBounds.x);
        currentPos.y = Mathf.Clamp(currentPos.y, -moveBounds.y, moveBounds.y);
        transform.position = currentPos;

        // 5. Apply banking roll visual when turning
        Quaternion targetRotation = Quaternion.Euler(-inputVector.y * bankAmount, 0f, -inputVector.x * bankAmount);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothTime);

        // 6. Continuous subtle engine rumble shake
        if (cockpitCameraTransform != null)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * engineShakeAmount;
            cockpitCameraTransform.localPosition = _originalCamPos + shakeOffset;
        }
    }
}