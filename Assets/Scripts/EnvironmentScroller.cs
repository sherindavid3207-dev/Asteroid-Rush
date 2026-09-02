using UnityEngine;

public class EnvironmentScroller : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;
    [SerializeField] private float resetZ = -200f;
    [SerializeField] private float startZ = 400f;

    private void Update()
    {
        // Move environment toward and behind the stationary cockpit camera
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime), Space.World);

        // Loop environment back ahead when it passes behind the camera
        if (transform.position.z < resetZ)
        {
            Vector3 pos = transform.position;
            pos.z = startZ;
            transform.position = pos;
        }
    }
}