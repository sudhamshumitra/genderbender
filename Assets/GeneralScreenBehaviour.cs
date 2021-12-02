using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralScreenBehaviour : MonoBehaviour
{ 
    [SerializeField] private float fadeinTime;
    
    [SerializeField] private TextMeshProUGUI topText;
    [SerializeField] private TextMeshProUGUI promptResultantText;
    [SerializeField] private TextMeshProUGUI originalLineText;
    [SerializeField] private TextMeshProUGUI originalAnswerText;

    [SerializeField] private string originPromptAnswer;
    [SerializeField] private string originalPromptLine;
    [SerializeField] private string originalFadeInText;
    [SerializeField] private string updatedPromptText;
    [SerializeField] private string updatedFadeInText;
    
    private bool isAlreadyFlipped = false;
    
    public void SetupText(string originalPromptLine, string originalPromptAnswer, string originFadein, string updatedPrompt, string updatedFadein)
    {
        this.originalPromptLine = originalPromptLine;
        this.originPromptAnswer = originalPromptAnswer;
        originalFadeInText = originFadein;
        updatedPromptText = updatedPrompt;
        updatedFadeInText = updatedFadein;
        StartScreen();
    }

    [SerializeField]
    private float flipDuration;
    [SerializeField]
    private Image targetImage;

    private Coroutine flippingRoutine;
    public void FlipPrompt()
    {
        if (!isAlreadyFlipped)
        {
            isAlreadyFlipped = true;
            promptResultantText.transform.parent.GetComponent<Button>().interactable = false;

            originalLineText.gameObject.SetActive(false);
            originalAnswerText.gameObject.SetActive(false);
            promptResultantText.gameObject.SetActive(true);
            topText.text = string.Empty;
            promptResultantText.text = string.Empty;
            
            flippingRoutine = StartCoroutine(FlipImage(flipDuration, targetImage,() =>
            {
                promptResultantText.text = updatedPromptText;
                topText.color = new Color(topText.color.r, topText.color.g, topText.color.b, 0);
                topText.text = updatedFadeInText;
                TextFadein();
            }));

        }
    }

    private void StartScreen()
    {
        
        originalLineText.gameObject.SetActive(true);
        originalAnswerText.gameObject.SetActive(true);
        promptResultantText.gameObject.SetActive(false);

        this.originalAnswerText.text = originPromptAnswer;
        this.originalLineText.text = originalPromptLine;
        
        var mid = topText.color;
        topText.color = new Color(mid.r, mid.g, mid.b, 0);
        topText.text = originalFadeInText;
        TextFadein();
        isAlreadyFlipped = false;
        if (flippingRoutine != null)
        {
            StopCoroutine(flippingRoutine);
        }
        targetImage.transform.localRotation = Quaternion.identity;
        promptResultantText.transform.parent.GetComponent<Button>().interactable = true;
    }

    private IEnumerator FlipImage(float t, Image i, Action onComplete)
    {
        var eulerAngle = i.transform.localRotation.eulerAngles;

        while (eulerAngle.y < 180.0f)
        {
            eulerAngle = i.transform.localRotation.eulerAngles;
            i.transform.localRotation =
                Quaternion.Euler(eulerAngle.x, eulerAngle.y + (Time.deltaTime / t) * (180.0f), eulerAngle.z);
            yield return null;
        }

        i.transform.localRotation = Quaternion.Euler(0, 0, 0);
        onComplete();
    }
    
    private void TextFadeout()
    {
        StartCoroutine(FadeTextToZeroAlpha(fadeinTime, topText));
    }
    private void TextFadein()
    {
        StartCoroutine(FadeTextToFullAlpha(fadeinTime, topText));
    }
    
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
