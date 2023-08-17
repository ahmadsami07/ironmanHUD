using UnityEngine;
using UnityEngine.UI;

public class ShimmerAnimation : MonoBehaviour
{
    public float shimmerSpeed = 1f; // Adjust the speed of the shimmer effect
    public float shimmerIntensity = 1f; // Adjust the intensity of the shimmer effect

    private RawImage rawImage;
    private Material material;
    private float offset;

    private void Start()
    {
        rawImage = GetComponent<RawImage>();
        material = rawImage.material;
        offset = 0f;
    }

    private void Update()
    {
        offset += Time.deltaTime * shimmerSpeed;
        material.SetFloat("_Offset", offset);
        material.SetFloat("_Intensity", shimmerIntensity);
    }
}
