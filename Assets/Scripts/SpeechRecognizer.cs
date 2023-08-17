using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static SpeechRecognizerPlugin;

public class SpeechRecognizer : MonoBehaviour, ISpeechRecognizerPlugin
{
    [SerializeField] private RawImage image = null;
    [SerializeField] private Image circularFill = null;
    [SerializeField] private TextMeshProUGUI batteryLevelText = null;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject rotatingObject = null;
    [SerializeField] private GameObject powerLevelObject = null;
    [SerializeField] private float rotationSpeed = 50f;
    private Vector3 powerLevelObjectStartPosition;
    private Vector3 powerLevelObjectStartScale = Vector3.zero;
    private Vector3 powerLevelObjectTargetScale = Vector3.one;
    [SerializeField] private float popInDuration = 1f;
    [SerializeField] private AnimationCurve popInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    private float currentAlpha = 1f;
    private float fadeTimer = 0f;
    

    private SpeechRecognizerPlugin plugin = null;

    private void Start()
    {
        plugin = SpeechRecognizerPlugin.GetPlatformPluginVersion(this.gameObject.name);

        // startListeningBtn.onClick.AddListener(StartListening);
        plugin.StartListening();
        // stopListeningBtn.onClick.AddListener(StopListening);
        // continuousListeningTgle.onValueChanged.AddListener(SetContinuousListening);
        // // StartListening();
        // // SetContinuousListening(true);
        // languageDropdown.onValueChanged.AddListener(SetLanguage);
        // maxResultsInputField.onEndEdit.AddListener(SetMaxResults);

        powerLevelObjectStartPosition = powerLevelObject.transform.position;
        powerLevelObject.transform.localScale = powerLevelObjectStartScale;
    }

     void StartListening()
    {
        Debug.Log("Start listening clickedd");
        plugin.StartListening();
    }

    private void StopListening()
    {
        plugin.StopListening();
    }

    private void SetContinuousListening(bool isContinuous)
    {
        plugin.SetContinuousListening(isContinuous);
    }

    private void SetLanguage(int dropdownValue)
    {
        string newLanguage = languageDropdown.options[dropdownValue].text;
        plugin.SetLanguageForNextRecognition(newLanguage);
    }

    private void SetMaxResults(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            return;

        int maxResults = int.Parse(inputValue);
        plugin.SetMaxResultsForNextRecognition(maxResults);
    }

    public void OnResult(string recognizedResult)
    {
        char[] delimiterChars = { '~' };
        string[] result = recognizedResult.Split(delimiterChars);

        

        resultsTxt.text = "";
        for (int i = 0; i < result.Length; i++)
        {
            resultsTxt.text += result[i] + '\n';

            if (result[i].ToLower().Contains("engage"))
            {
                EngageHUD(); // Call the EngageHUD method when the word "engage" is detected
            }
            else if (result[i].ToLower().Contains("shutdown"))
            {
                DisengageHUD(); // Call the EngageHUD method when the word "engage" is detected
            }
            else if (result[i].ToLower().Contains("power"))
            {
                RetrievePowerLevel(); // Call the EngageHUD method when the word "engage" is detected
            }
        }
    }

    public void EngageHUD()
    {
        rotatingObject.SetActive(true);
        StartCoroutine(FadeOutImage());
    }

    public void DisengageHUD()
    {
        image.gameObject.SetActive(true); 

    }

    public void RetrievePowerLevel()
    {
        Debug.Log("Retrieving power level");
        StartCoroutine(PopInPowerLevelObject());
        float batteryLevel = SystemInfo.batteryLevel*100;
        StartCoroutine(CountBatteryLevel(batteryLevel));
    }



    private IEnumerator CountBatteryLevel(float targetLevel)
    {
        float currentLevel = 0f;
        float fillAmount = 0f;

        while (currentLevel <= targetLevel)
        {
            batteryLevelText.text = currentLevel.ToString();
            fillAmount = currentLevel / targetLevel;
            SetCircularFillAmount(fillAmount); 
            currentLevel += 1f;
            yield return new WaitForSeconds(0.0001f); // Adjust the delay based on your desired animation speed
        }

        batteryLevelText.text = targetLevel.ToString();
        StartCoroutine(FadeOutPowerLevelObject());

    }

    private void SetCircularFillAmount(float fillAmount)
    {
    circularFill.fillAmount = fillAmount;
    }

    private IEnumerator FadeOutImage()
    {
    fadeTimer = 0f;
    currentAlpha = 1f;

    Color startColor = image.color;
    Color endColor = startColor;
    endColor.a = 0f;

    while (fadeTimer < fadeDuration)
    {
        fadeTimer += Time.deltaTime;
        float normalizedTime = fadeTimer / fadeDuration;
        Color newColor = Color.Lerp(startColor, endColor, normalizedTime);
        image.color = newColor;

        rotatingObject.transform.Rotate(Vector3.forward* rotationSpeed * Time.deltaTime);

        yield return null;
    }

    image.gameObject.SetActive(false); // Disable the RawImage once the fade-out is complete
    rotatingObject.SetActive(false);
    }

    private Vector3 CalculateStartPosition()
    {
        // Get the screen's width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the position in the middle of the screen
        Vector3 startPosition = new Vector3(screenWidth * 0.75f, screenHeight * 0.5f, 0f);

        return startPosition;
        }

    private IEnumerator PopInPowerLevelObject()
    {
        // Move the powerLevelObject to its start position (top of the screen)
        powerLevelObject.transform.position = CalculateStartPosition();

        // Activate the powerLevelObject to make it visible
        powerLevelObject.SetActive(true);

        // Perform the pop-in animation
        float timer = 0f;
        while (timer < popInDuration)
        {
            float normalizedTime = timer / popInDuration;
            float scaleFactor = popInCurve.Evaluate(normalizedTime);
            powerLevelObject.transform.localScale = powerLevelObjectStartScale + (powerLevelObjectTargetScale - powerLevelObjectStartScale) * scaleFactor;

            timer += Time.deltaTime;
            yield return null;
        }

        // Set the final scale
        powerLevelObject.transform.localScale = powerLevelObjectTargetScale;
    }

    private IEnumerator PopOutPowerLevelObject()
    {
    // Perform the pop-out animation
    float timer = 0f;
    while (timer < popInDuration)
    {
        float normalizedTime = timer / popInDuration;
        float scaleFactor = popInCurve.Evaluate(1f - normalizedTime);
        powerLevelObject.transform.localScale = powerLevelObjectTargetScale - (powerLevelObjectTargetScale - powerLevelObjectStartScale) * scaleFactor;

        timer += Time.deltaTime;
        yield return null;
    }

    // Deactivate the powerLevelObject to hide it
    powerLevelObject.SetActive(false);
    }

    private IEnumerator FadeOutPowerLevelObject()
    {
    // Perform the fade-out animation
    float timer = 0f;
    Color startColor = powerLevelObject.GetComponent<Renderer>().material.color;
    Color endColor = startColor;
    endColor.a = 0f;

    while (timer < fadeDuration)
    {
        timer += Time.deltaTime;
        float normalizedTime = timer / fadeDuration;
        Color newColor = Color.Lerp(startColor, endColor, normalizedTime);
        powerLevelObject.GetComponent<Renderer>().material.color = newColor;

        yield return null;
    }

    // Deactivate the powerLevelObject to hide it
    powerLevelObject.SetActive(false);
    }




    public void OnError(string recognizedError)
    {
        ERROR error = (ERROR)int.Parse(recognizedError);
        switch (error)
        {
            case ERROR.UNKNOWN:
                Debug.Log("<b>ERROR: </b> Unknown");
                errorsTxt.text += "Unknown";
                break;
            case ERROR.INVALID_LANGUAGE_FORMAT:
                Debug.Log("<b>ERROR: </b> Language format is not valid");
                errorsTxt.text += "Language format is not valid";
                break;
            default:
                break;
        }
    }
}
