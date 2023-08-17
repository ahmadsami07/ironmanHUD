using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARFace))]
public class FaceObjectController : MonoBehaviour
{
    private ARFace arFace;

    private void Awake()
    {
        arFace = GetComponent<ARFace>();
    }

    private void OnEnable()
    {
        arFace.updated += OnFaceUpdated;
    }

    private void OnDisable()
    {
        arFace.updated -= OnFaceUpdated;
    }

    private void OnFaceUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        // Update the position and rotation of the face object based on the detected face
        transform.position = arFace.transform.position;
        transform.rotation = arFace.transform.rotation;
    }
}
