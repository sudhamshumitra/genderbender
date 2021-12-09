using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SwipeManager : MonoBehaviour
{
    private Coroutine fadeinRoutine;

    [SerializeField] private Image curtainImage;
    [SerializeField] private float curtainFoldingDuration;

    [SerializeField]
    private GameObject _leftObject;
    [SerializeField]
    private GameObject _rightObject;
    
    [SerializeField] private ExperienceDataScriptableObject experienceData;

    [Header("DirectionScreen")] [SerializeField]
    private TextMeshProUGUI directionScreenText;
    
    [SerializeField] private CardBehaviour swipePrefab;
    [SerializeField] private Transform swipeCardParent;
    [SerializeField] private CardDataScriptableObject cardData;

    [SerializeField] private GameObject bandScreen;
    [SerializeField] private GameObject finalScreen;
    [SerializeField] private GameObject videoScreen;
    [SerializeField] private GameObject profileSelect;
    [SerializeField] private GameObject chatScreen;
    [SerializeField] private GameObject generalScreen;
    [SerializeField] private GameObject generalScreenCompleteImageChangeScreen;
    [SerializeField] private GameObject generalScreenBGImageChangeScreen;
    [SerializeField] private GameObject directionScreen;
    
    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;

    [SerializeField] private Button heartButton;
    [SerializeField] private Button crossButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private int selectedCardIndex;
    [SerializeField] private int currentPageNumber = 0;
    [SerializeField] private int currentCardIndex = 0;
    
    private void FadeInCurtain()
    {
        fadeinRoutine = StartCoroutine(FadeTextToZeroAlpha(curtainFoldingDuration, curtainImage));
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Image i)
    {
        curtainImage.gameObject.SetActive(true);
        curtainImage.color = new Color(1f, 1, 1, 0.6f);
        _leftObject.gameObject.SetActive(false);
        _rightObject.gameObject.SetActive(false);
        
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

         
        _rightObject.gameObject.SetActive(true);
        _leftObject.gameObject.SetActive(true);


        if (experienceData!=null && ((experienceData.pageType.Count -2) == currentPageNumber))
        {
            _rightObject.gameObject.SetActive(false);   
        }

        curtainImage.gameObject.SetActive(false);
    }
    
    private void ClickAccept()
    {
        if (cardData.IsSamplingExperienceData) ClickReject();
        selectedCardIndex = currentCardIndex;
        experienceData = cardData.CardDataCollection[currentCardIndex].experienceData;
        currentPageNumber +=1;
        UpdateScreen();
    }

    private void ClickReject()
    {
        currentCardIndex += 1;
        currentCardIndex %= cardData.CardDataCollection.Length;
        
        //currentCardIndex -= 1;
        if (currentCardIndex < 0) currentCardIndex += cardData.CardDataCollection.Length;
        // currentPageNumber  cardData.CardDataCollection.Length;
        
        cardObject.SetupCard(cardData.CardDataCollection[currentCardIndex]);
    }
    
    
    private void ShowGeneralScreen(GeneralScreenData generalScreenData)
    {
        TurnOffAllScreens();
        generalScreen.gameObject.SetActive(true);
        generalScreen.GetComponent<GeneralScreenBehaviour>().SetupText(
            generalScreenData.originalPromptLine,
            generalScreenData.originalPromptAnswer,
            generalScreenData.originalTopText, 
            generalScreenData.updatedPromptText, 
            generalScreenData.updatedTopText);
            chatScreen.gameObject.SetActive(false);
    }

    private void TurnOffAllScreens()
    {
        _leftObject = leftBtn.gameObject;
        _rightObject = rightBtn.gameObject;
        
        _leftObject.gameObject.SetActive(false);
        _rightObject.gameObject.SetActive(false);
        crossButton.gameObject.SetActive(false);
        heartButton.gameObject.SetActive(false);
        videoScreen.gameObject.SetActive(false);
        chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(false);
        generalScreen.gameObject.SetActive(false);
        generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);
        bandScreen.gameObject.SetActive(false);
        finalScreen.gameObject.SetActive(false);
       // generalScreenBGImageChangeScreen.gameObject.SetActive(false);   
        directionScreen.gameObject.SetActive(false);
    }

    private void ShowChatScreen(string profileName, ProfileImage profileDP, List<ChatElement> chatElements)
    {
        TurnOffAllScreens();
        chatScreen.GetComponent<ChatPlayer>()._chatData = new ChatData(profileName, profileDP, chatElements);
        chatScreen.gameObject.SetActive(true);
    }

    private void ShowDirectionScreen(string directionScreenSaysThis)
    {
        TurnOffAllScreens();
        directionScreenText.text = directionScreenSaysThis;
        directionScreen.gameObject.SetActive(true);
    }

    private void ShowGeneralCompleteImageScreen(ProfileImage[] profileImages)
    {
        TurnOffAllScreens();
        generalScreenCompleteImageChangeScreen
            .GetComponent<GeneralImageScreenBehaviour>()
            .SetupGeneralImageScreen(
                new GeneralImageScreenBehaviour.Data(profileImages));

        generalScreenCompleteImageChangeScreen.gameObject.SetActive(true);
    }

    private void ShowBandScreen(List<SpotifyBandInfo> spotifyBandInfos)
    {
        TurnOffAllScreens();
        
        bandScreen
            .GetComponent<BandScreen>()
            .SetupBandScreen("Drag and drop each “Interest” into the box to see what each of these are composed of. Or what we are composed of. ", spotifyBandInfos);
        
        bandScreen.gameObject.SetActive(true);
    }
    
    private void ShowProfileScreen()
    {
        TurnOffAllScreens();
        _leftObject = crossButton.gameObject;
        _rightObject = heartButton.gameObject;
        
        profileSelect.gameObject.SetActive(true);
    }

    private void ShowFinalScreen()
    {
        TurnOffAllScreens();
        finalScreen.gameObject.SetActive(true);
    }
    
    private void ShowVideoScreen()
    {
        TurnOffAllScreens();
        videoScreen.GetComponent<VideoPlayer>().url = System.IO.Path.Combine (Application.streamingAssetsPath,experienceData.swipeVideoName + ".mp4"); 
        videoScreen.gameObject.SetActive(true);
    }

    private int directionIndex;
    private int promptIndex;
    private int dragDropIndex;
    private int imageChangeIndex;
    
    private void UpdateScreen()
    {
        if (currentPageNumber < 3)
        {
            switch (currentPageNumber)
            {
                case 0:
                    ShowDirectionScreen(
                        "Welcome to //CasteNoBar. <br><br>We have multiple narratives that you can choose from and experience by 'hearting'..<br><br> Click on the bottom right arrow to continue");
                    break;
                case 1:
                    ShowDirectionScreen(
                        "2nd direction screen goes here");
                    break;
                case 2:
                    ShowProfileScreen();
                    break;
            }
        }
        else
        {
            restartButton.gameObject.SetActive(false);

            var pageData = experienceData.pageType[currentPageNumber];
            switch (pageData.pageType)
            {
                case EPageType.SCREEN_DIRECTION:
                    ShowDirectionScreen(experienceData.directionScreens[pageData.mapIndex]);
                    break;
                case EPageType.SCREEN_GENERALPROMPTSCREEN:
                    ShowGeneralScreen(experienceData.GeneralScreenDataCollection[pageData.mapIndex]);
                    break;
                case EPageType.SCREEN_IMAGECHANGE:
                    ShowGeneralCompleteImageScreen(experienceData.spriteChange[pageData.mapIndex].profileImages);
                    break;
                case EPageType.SCREEN_DRAGDROP:
                    ShowBandScreen(experienceData.spotifyData.bandInfo);
                    break;
                case EPageType.SCREEN_PROFILE:
                    ShowProfileScreen();
                    break;
                case EPageType.SCREEN_VIDEO:
                    ShowVideoScreen();
                    break;
                case EPageType.SCREEN_CHAT:
                    ShowChatScreen(experienceData.selectedProfileName, experienceData.selectedProfileDP, experienceData.chatData[pageData.mapIndex].chatElements);
                    break;
                case EPageType.SCREEN_RESTART:
                    ShowFinalScreen();
                    rightBtn.gameObject.SetActive(false);
                    restartButton.gameObject.SetActive(true);
                    break;
                case EPageType.NONE:
                    throw new ArgumentOutOfRangeException();
                    break;
                case EPageType.MAX:
                    throw new ArgumentOutOfRangeException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }
        
        void FadeInScreen()
        {
            if (fadeinRoutine != null)
            {
                StopCoroutine(fadeinRoutine);
            }
            FadeInCurtain();
        }
        FadeInScreen();
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
        restartButton.onClick.AddListener(RestartScene);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(0);
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
