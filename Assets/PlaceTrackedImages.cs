
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImages : MonoBehaviour {
    // Reference to AR tracked image manager component
    private ARTrackedImageManager _trackedImagesManager;

    // List of prefabs to instantiate - these should be named the same
    // as their corresponding 2D images in the reference image library 
    public GameObject[] ArPrefabs;

    // Keep dictionary array of created prefabs
    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    void Awake() {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable() {
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable() {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {

        // Loop through all new tracked images that have been detected
        foreach (var trackedImage in eventArgs.added) {
            var imageName = trackedImage.referenceImage.name;
            // Now loop over the array of prefabs
            foreach (var curPrefab in ArPrefabs) {
                if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0
                    && !_instantiatedPrefabs.ContainsKey(imageName)) {
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    // Add the created prefab to our array
                    _instantiatedPrefabs[imageName] = newPrefab;
                }
            }
        }

        foreach (var trackedImage in eventArgs.updated) {
            _instantiatedPrefabs[trackedImage.referenceImage.name]
                .SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }

        foreach (var trackedImage in eventArgs.removed) {
            // Destroy its prefab
            Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
            // Also remove the instance from our array
            _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);

        }
    }
}