using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class UpperHUDVisibilityScript : MonoBehaviour
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

        keyActs.Add("engage", Engage);
        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
        recognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Command: " + args.text);
        keyActs[args.text].Invoke();
    }

    void Engage()
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

    }

    private void SetImageVisibility(bool isVisible)
    {
        targetImage.enabled = isVisible;

        if (isVisible)
        {
            StartFade();
        }
        else
        {
        }
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

}

