using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    [Header("Forward Speed Visual")]
    [SerializeField] private float rollSpeed = 15f; // Speed of the forward skybox tumble

    private Matrix4x4 _skyboxMatrix;

    private void Update()
    {
        // Rotate around the X-axis so stars/nebulae roll overhead toward you
        float angle = Time.time * rollSpeed;
        
        // Build a custom rotation matrix pitching forward (X-axis)
        _skyboxMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(angle, 0f, 0f), Vector3.one);

        // Apply matrix directly to the active Skybox shader
        if (RenderSettings.skybox != null)
        {
            RenderSettings.skybox.SetMatrix("_Rotation", _skyboxMatrix);
        }
    }
}