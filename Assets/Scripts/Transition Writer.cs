using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterEffectWithFadeAndVariableSpeed : MonoBehaviour
{
    public TMP_Text uiText; // Reference to the UI Text component
    public string firstText; // The first text to display
    public string secondText; // The second text to display
    public float[] typeDelays; // Array of delays between each character
    public float fadeDuration = 1.0f; // Duration of the fade effect
    public string nextSceneName;

    private string currentText = "";
    public SceneFade sceneFade;

    void Start()
    {
        StartCoroutine(DisplayText());
    }

    void Update()
    {
        if (currentText == secondText)
        {
            sceneFade.FadeToScene(nextSceneName);
        }
    }

    IEnumerator DisplayText()
    {
        yield return StartCoroutine(ShowText(firstText));
        yield return StartCoroutine(FadeOutText());
        yield return StartCoroutine(ShowText(secondText));
    }

    IEnumerator ShowText(string text)
    {
        currentText = "";
        uiText.text = currentText;

        for (int i = 0; i < text.Length; i++)
        {
            currentText = text.Substring(0, i + 1);
            uiText.text = currentText;
            float delay = typeDelays[i % typeDelays.Length];
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator FadeOutText()
    {
        Color originalColor = uiText.color;
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            uiText.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / fadeDuration));
            yield return null;
        }
        uiText.color = originalColor;
        uiText.text = "";
    }
}
