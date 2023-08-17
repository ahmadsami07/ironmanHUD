using UnityEngine;

public class RotatingTarget : MonoBehaviour
{
    public float rotationSpeed = 30f;

    private void Update()
    {
        // Rotate the prefab here in Update if you want it to rotate all the time
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnBecameVisible()
    {
        // Called when the prefab becomes visible by any camera
        // Start rotating the prefab
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        // Called when the prefab becomes invisible by any camera
        // Stop rotating the prefab
        enabled = false;
    }
}
