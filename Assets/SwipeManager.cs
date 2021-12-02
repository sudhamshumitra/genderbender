using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwipeManager : MonoBehaviour
{
    private Coroutine fadeinRoutine;
    
    [SerializeField] private Image curtainImage;
    [SerializeField] private float curtainFoldingDuration;
    private void FadeInCurtain()
    {
        fadeinRoutine = StartCoroutine(FadeTextToZeroAlpha(curtainFoldingDuration, curtainImage));
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Image i)
    {
        curtainImage.gameObject.SetActive(true);
        curtainImage.color = new Color(1f, 1, 1, 0.6f);
        leftBtn.gameObject.SetActive(false);
        rightBtn.gameObject.SetActive(false);
        
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        
        leftBtn.gameObject.SetActive(true);
        rightBtn.gameObject.SetActive(true);
        curtainImage.gameObject.SetActive(false);
    }

   

    [SerializeField] private ExperienceDataScriptableObject _experienceDataScriptableObject;

    [Header("DirectionScreen")] [SerializeField]
    private TextMeshProUGUI directionScreenText;
    
    [SerializeField] private CardBehaviour swipePrefab;
    [SerializeField] private Transform swipeCardParent;
    [SerializeField] private CardDataScriptableObject cardData;

    [SerializeField] private GameObject videoScreen;
    [SerializeField] private GameObject profileSelect;
    [SerializeField] private GameObject chatScreen;
    [SerializeField] private GameObject generalScreen;
    [SerializeField] private GameObject generalScreenCompleteImageChangeScreen;
    [SerializeField] private GameObject generalScreenBGImageChangeScreen;
    [SerializeField] private GameObject directionScreen;

    [Header("Icon")] [SerializeField] private Sprite leftButtonIcon;
    [SerializeField] private Sprite rightButtonIcon;

    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;

    [SerializeField] private Button heartButton;
    [SerializeField] private Button crossButton;

    [SerializeField] private int selectedCardIndex;
    [SerializeField] private int currentPageNumber = 0;
    [SerializeField] private int currentCardIndex = 0;
    
    private void ClickAccept()
    {
        selectedCardIndex = currentCardIndex;
        currentCardIndex = 1;
        UpdateScreen();
    }

    private void ClickReject()
    {
        // currentCardIndex += 1;
        // currentCardIndex %= cardData.CardDataCollection.Length;
        
        currentCardIndex -= 1;
        if (currentCardIndex < 0) currentCardIndex += cardData.CardDataCollection.Length;
        // currentPageNumber  cardData.CardDataCollection.Length;
        
        cardObject.SetupCard(cardData.CardDataCollection[currentCardIndex]);
    }

    private void ShowGeneralScreen(
        string originalPromptLine,
        string originalPromptAnswer,
        string originalTopText,
        string updatedPromptText,
        string updatedTopText)
    {
        TurnOffAllScreens();
        generalScreen.gameObject.SetActive(true);
        generalScreen.GetComponent<GeneralScreenBehaviour>().SetupText(
            originalPromptLine,
            originalPromptAnswer,
            originalTopText, updatedPromptText, updatedTopText);
            chatScreen.gameObject.SetActive(false);
    }

    private void TurnOffAllScreens()
    {
        videoScreen.gameObject.SetActive(false);
        chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(false);
        generalScreen.gameObject.SetActive(false);
        generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);
        generalScreenBGImageChangeScreen.gameObject.SetActive(false);   
        directionScreen.gameObject.SetActive(false);
    }

    private void ShowChatScreen(List<ChatElement> chatElements)
    {
        TurnOffAllScreens();
        chatScreen.GetComponent<ChatPlayer>()._chatData = new ChatData(chatElements);
        chatScreen.gameObject.SetActive(true);
    }

    private void ShowDirectionScreen(string directionScreenSaysThis)
    {
        TurnOffAllScreens();
        directionScreenText.text = directionScreenSaysThis;
        directionScreen.gameObject.SetActive(true);
    }

    private void ShowGeneralCompleteImageScreen()
    {
        TurnOffAllScreens();
        generalScreenCompleteImageChangeScreen
            .GetComponent<GeneralImageScreenBehaviour>()
            .SetupGeneralImageScreen(
                new GeneralImageScreenBehaviour.Data(cardData.CardDataCollection[selectedCardIndex]
                    .changeSpritesInComplete));

        generalScreenCompleteImageChangeScreen.gameObject.SetActive(true);
    }

    private void ShowProfileScreen()
    {
        TurnOffAllScreens();
        profileSelect.gameObject.SetActive(true);
    }

    private void ShowVideoScreen()
    {
        TurnOffAllScreens();
        videoScreen.gameObject.SetActive(true);
    }

    private void UpdateScreen()
    {

        void FadeInScreen()
        {
            if (fadeinRoutine != null)
            {
                StopCoroutine(fadeinRoutine);
            }
            FadeInCurtain();

        }
        
        FadeInScreen();
        switch (currentPageNumber)
        {
            case 0:
                ShowDirectionScreen(
                    "Welcome to //CasteNoBar. <br><br>Click on the arrow key at the bottom to go to the next screen!");
                break;

            case 1:
                ShowProfileScreen();
                break;

            case 2:
                ShowDirectionScreen(
                    "You've chosen the profile of Kritika. <br><br>Let's have a look at her profile. Click on the arrow at the bottom!");
                break;

            case 3:
                ShowDirectionScreen("Click on the arrow for a closer look at some of the prompts she has chosen");
                break;

            case 4:
                ShowGeneralScreen(
                    "A conspiracy theory I believe in...",
                    "The porn ban was the downfall of tumblr",
                    "Tap to see what this prompt is trying to reveal",
                    "You know it, we know it, Kritika knows it. We’re constantly hoping to connect on our familiarity with pop culture, and Western pop culture at that. It’s not as if the idea of conspiracy theory is a heavily guarded secret in the varna system... but the prompt itself is invested in showing us how close someone is to the kind of culture you are consuming.",
                    "Click on the bottom arrow to continue!"
                );
                break;
            case 5:
                // TODO: 
                ShowVideoScreen();
                break;

            case 6:
                // TODO: Add parameter as string or empty
                ShowDirectionScreen(
                    "Kritika finds this prompt witty. Tap on the prompt in the next screen to unpack why.");
                break;

            case 7:
                ShowGeneralScreen(
                    "Change my mind about",
                    "Blended whiskey. It’s entirely underrated.",
                    "Tap to see what this prompt is trying to reveal",
                    "We, like Kritika, may have connected on alcohol reserves at home, or some other sign of liberal households... or simply markers that tell us who is upwardly mobile, who is of the same class as us, who can afford the same lifestyle as us.",
                    "Click on the bottom arrow to continue!"
                );

                break;

            case 8:
                ShowDirectionScreen(
                    "Images matter too. So tap on the next image to see what would happen if the background changed");
                break;

            case 9:
                ShowGeneralCompleteImageScreen();
                break;
            case 10:
                ShowDirectionScreen("A peek into her conversation with Abhijith.");
                break;
            case 11:
                ShowChatScreen(_experienceDataScriptableObject.chatData1);
                break;
            case 12:
                ShowDirectionScreen(
                    "As they bond about whiskey collections, they also connect on their “backgrounds”...");
                break;
            case 13:
                ShowChatScreen(_experienceDataScriptableObject.chatData2);
                break;
            case 14:
                ShowDirectionScreen("Small world indeed.");
                break;
            case 15:
                ShowDirectionScreen("We will be adding more narratives soon! :)");
                break;
        }

        leftBtn.gameObject.SetActive(true);
        rightBtn.gameObject.SetActive(true);
        heartButton.gameObject.SetActive(false);
        crossButton.gameObject.SetActive(false);
        
        // if (currentPageNumber > 0)
        // {
        //     leftBtn.gameObject.SetActive(true);
        //     rightBtn.gameObject.SetActive(true);
        //     heartButton.gameObject.SetActive(false);
        //     crossButton.gameObject.SetActive(false);
        // }
        // else
        // {
        //     leftBtn.gameObject.SetActive(false);
        //     rightBtn.gameObject.SetActive(false);
        //     heartButton.gameObject.SetActive(true);
        //     crossButton.gameObject.SetActive(true);
        // }
    }
    
    private void ClickNext()
    {
        currentPageNumber +=1;
        UpdateScreen();
    }

    private void OnDisable()
    {
        StopCoroutine(fadeinRoutine);
    }

    private void ClickPrevious()
    {
        if (currentPageNumber > 2)
        {
            currentPageNumber -= 1;
        }  
        UpdateScreen();
    }
    
    private void SetupButtons()
    {
        heartButton.onClick.AddListener(ClickAccept);
        crossButton.onClick.AddListener(ClickReject);
        leftBtn.onClick.AddListener(ClickPrevious);
        rightBtn.onClick.AddListener(ClickNext);
    }
    
    
    [SerializeField] private GameObject frameObject;
    
    private void Start()
    {
        SetupButtons();
        Initiate();
        cardObject.SetupCard(cardData.CardDataCollection[currentPageNumber]);
        AdjustFrame();
        UpdateScreen();
    }

    private void AdjustFrame()
    {
        frameObject.transform.SetAsLastSibling();
    }

    private CardBehaviour cardObject;
    
    private void Initiate()
    {
        cardObject = Instantiate(swipePrefab, swipeCardParent).GetComponent<CardBehaviour>();
        cardObject.GetComponent<SwipeEffect>().enabled = false;
        swipeCardParent.GetComponent<ScrollRect>().content = cardObject.GetComponent<RectTransform>();
    }
}
