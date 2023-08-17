using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingVisibility : MonoBehaviour
{
   private Renderer sphereRenderer;

    private void Start()
    {
        // Get the Renderer component of the sphere
        sphereRenderer = GetComponent<Renderer>();

        // Set the alpha value of the sphere's material to 0 (transparent)
        Color sphereColor = sphereRenderer.material.color;
        sphereColor.a = 0f;
        sphereRenderer.material.color = sphereColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
