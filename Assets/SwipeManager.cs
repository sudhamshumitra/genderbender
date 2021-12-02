using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeManager : MonoBehaviour
{

    [SerializeField] private ExperienceDataScriptableObject _experienceDataScriptableObject;
    
    [SerializeField] private CardBehaviour swipePrefab;
    [SerializeField] private Transform swipeCardParent;
    [SerializeField] private CardDataScriptableObject cardData;

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

    private void ClickAccept()
    {
        selectedCardIndex = currentPageNumber;
        currentPageNumber = 1;
        UpdateScreen();
    }

    private void ClickReject()
    {
        currentPageNumber += 1;
        currentPageNumber %= cardData.CardDataCollection.Length;
        cardObject.SetupCard(cardData.CardDataCollection[currentPageNumber]);
    }

    private void ShowGeneralScreen(
        string originalPromptLine,
        string originalPromptAnswer,
        string originalTopText,
        string updatedPromptText,
        string updatedTopText)
    {
        generalScreen.gameObject.SetActive(true);
        generalScreen.GetComponent<GeneralScreenBehaviour>().SetupText(
            originalPromptLine,
            originalPromptAnswer,
            originalTopText, updatedPromptText, updatedTopText);
            chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(false);
        generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);
        generalScreenBGImageChangeScreen.gameObject.SetActive(false);
    }

    private void TurnOffAllScreens()
    {
        chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(false);
        generalScreen.gameObject.SetActive(false);
        generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);
        generalScreenBGImageChangeScreen.gameObject.SetActive(false);   
    }

    private void ShowChatScreen(List<ChatElement> chatElements)
    {
        TurnOffAllScreens();
        chatScreen.gameObject.SetActive(true);
        chatScreen.GetComponent<ChatPlayer>()._chatData = new ChatData(chatElements);
    }

    private void ShowDirectionScreen(string directionScreenSaysThis)
    {
        directionScreen.gameObject.SetActive(true);
        chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(false);
        generalScreen.gameObject.SetActive(false);
    generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);
    generalScreenBGImageChangeScreen.gameObject.SetActive(false);
}

private void ShowGeneralCompleteImageScreen()
    {
        chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(false);
        generalScreen.gameObject.SetActive(false);
        
        generalScreenCompleteImageChangeScreen
            .GetComponent<GeneralImageScreenBehaviour>()
            .SetupGeneralImageScreen(
                new GeneralImageScreenBehaviour.Data(cardData.CardDataCollection[selectedCardIndex].changeSpritesInComplete));
        
        generalScreenCompleteImageChangeScreen.gameObject.SetActive(true);   
        generalScreenBGImageChangeScreen.gameObject.SetActive(false);
    }

    // private void ShowGeneralScreenImageBackground(Sprite[] spriteCollection)
    // {
    //     chatScreen.gameObject.SetActive(false);
    //     profileSelect.gameObject.SetActive(false);
    //     generalScreen.gameObject.SetActive(false);
    //     
    //     generalScreenBGImageChangeScreen
    //         .GetComponent<GeneralImageScreenBehaviour>()
    //         .SetupGeneralImageScreen(
    //             new GeneralImageScreenBehaviour.Data(cardData.CardDataCollection[selectedCardIndex].changeOnlyBG));
    //     
    //     generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);  
    //     generalScreenBGImageChangeScreen.gameObject.SetActive(true);
    // }
    
    private void ShowProfileScreen()
    {
        chatScreen.gameObject.SetActive(false);
        profileSelect.gameObject.SetActive(true);
        generalScreen.gameObject.SetActive(false);
        generalScreenCompleteImageChangeScreen.gameObject.SetActive(false);   
        generalScreenBGImageChangeScreen.gameObject.SetActive(false);
    }

    private void ShowVideoScreen()
    {
        // video here is implicit
    }
    
    private void UpdateScreen()
    {

        switch (currentPageNumber)
        {
            case 0:
                // TODO: only show kritika profile
                ShowProfileScreen();
                break;
            
            case 1:
                ShowDirectionScreen("Click on the arrow for a closer look at some of the prompts she has chosen");
                break;
                
        case 2:
                // TODO: 
                ShowVideoScreen();
        break;
                
            case 3:
                // TODO: Add parameter as string or empty
                ShowDirectionScreen("Kritika finds this prompt witty. Tap on the prompt in the next screen to unpack why.");
                break;
                
            case 4:
                ShowGeneralScreen(
                    "Change my mind about",
                    "Blended whiskey. It’s entirely underrated.",
                    "Tap to see what this prompt is trying to reveal",
                    "We, like Kritika, may have connected on alcohol reserves at home, or some other sign of liberal households... or simply markers that tell us who is upwardly mobile, who is of the same class as us, who can afford the same lifestyle as us.",
                    string.Empty
                );

                break;
            
            case 5:
                ShowDirectionScreen("Images matter too. So tap on the next image to see what would happen if the background changed");
                break;
            
            case 6:
                ShowGeneralCompleteImageScreen();
                break;
           
            case 7:
                ShowDirectionScreen("A peek into her conversation with Abhijith.");
                break;
            case 8:
                ShowChatScreen(_experienceDataScriptableObject.chatData1);
                break;
            case 9:
                ShowDirectionScreen("As they bond about whiskey collections, they also connect on their “backgrounds”...");
                break;
            case 10:
                ShowChatScreen(_experienceDataScriptableObject.chatData2);
                break;
            case 11:
                ShowDirectionScreen("Small world indeed.");
                break;
        }
        
        if (currentPageNumber > 0)
        {
            leftBtn.gameObject.SetActive(true);
            rightBtn.gameObject.SetActive(true);
            heartButton.gameObject.SetActive(false);
            crossButton.gameObject.SetActive(false);
        }
        else
        {
            leftBtn.gameObject.SetActive(false);
            rightBtn.gameObject.SetActive(false);
            heartButton.gameObject.SetActive(true);
            crossButton.gameObject.SetActive(true);
        }
    }
    
    private void ClickNext()
    {
        currentPageNumber +=1;
        UpdateScreen();
    }

    private void ClickPrevious()
    {
        if (currentPageNumber > 1)
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
    
    private void SetupCards()
    {
        // foreach (var cardItem in cardData.CardDataCollection)
        // {
            // var cardObject = Instantiate(swipePrefab, swipeCardParent).GetComponent<CardBehaviour>();
            // cardObject.GetComponent<SwipeEffect>().enabled = false;
            //
            // swipeCardParent.GetComponent<ScrollRect>().content = cardObject.GetComponent<RectTransform>();
            // cardObject.SetupCard(cardItem);

            // var sizeDelta = cardObject.transform.GetComponent<RectTransform>().rect;
            // float totalHeight = 0;
            // for (var i = 0; i < cardObject.transform.childCount; i++)
            // {
            //     totalHeight += cardObject.transform.GetChild(i).GetComponent<RectTransform>().rect.height;
            // }
            //
            // cardObject.transform.GetComponent<RectTransform>().rect
            //     .Set(sizeDelta.x, sizeDelta.y, sizeDelta.width, (totalHeight));
            
        // }   
        
        // AdjustFrame();
    }
}
