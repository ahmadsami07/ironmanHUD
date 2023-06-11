using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class CrosshairVisibilityScript : MonoBehaviour
{
    private bool imageVisible = false;
    public Image targetImage;
    public float rotationSpeed = 100f;

    public float fadeDuration = 2f;
    private bool isFading = false;
    private float fadeTimer = 0f;

    private Dictionary<string, Action> keyActs = new Dictionary<string, Action>();
    private KeywordRecognizer recognizer;

    //Vars needed for sound playback.
    private AudioSource soundSource;
    public AudioClip[] sounds;

    void Start()
    {
        targetImage = GetComponent<Image>();
        SetImageVisibility(imageVisible);
        soundSource = GetComponent<AudioSource>();

        keyActs.Add("power up suit engines", Boot);
        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
        recognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Command: " + args.text);
        keyActs[args.text].Invoke();
    }

    void Boot()
    {
        imageVisible = !imageVisible;
        SetImageVisibility(imageVisible);
    }

    void Update()
    {
     
        if (isFading)
        {
            UpdateFade();
        }

        if (imageVisible)
        {
            RotateImage();
        }
    }

    private void SetImageVisibility(bool isVisible)
    {
        targetImage.enabled = isVisible;

        if (isVisible)
        {
            StartFade();
            StartRotationAnimation();
        }
        else
        {
            StopRotationAnimation();
        }
    }

    private void RotateImage()
    {
        targetImage.rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    private void StartRotationAnimation()
    {
        StopRotationAnimation(); // Stop the animation if it's already playing
        targetImage.gameObject.AddComponent<RotationAnimation>();
    }

    private void StartFade()
    {
        isFading = true;
        fadeTimer = 0f;
        targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 0f);
    }

     private void UpdateFade()
    {
        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
        targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, alpha);

        if (fadeTimer >= fadeDuration)
        {
            isFading = false;
        }
    }

    private void StopRotationAnimation()
    {
        RotationAnimation rotationAnimation = targetImage.GetComponent<RotationAnimation>();
        if (rotationAnimation != null)
        {
            Destroy(rotationAnimation);
        }
    }
}

public class RotationAnimation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }
}