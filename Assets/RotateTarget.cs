using UnityEngine;

public class RotateOnInstantiate : MonoBehaviour
{
    public float rotationSpeed = 30f; // Adjust the speed of rotation

    private void Start()
    {
        // Start rotating the object when it is instantiated
        RotateObject();
    }

    private void RotateObject()
    {
        // Rotate the object continuously based on the rotationSpeed
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
