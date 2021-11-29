using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SwipeManager : MonoBehaviour
{
    private int currentScreenIndex;

    [SerializeField]
    private CardBehaviour swipePrefab;
    [SerializeField]
    private Transform swipeCardParent;
    [SerializeField]
    private CardDataScriptableObject cardData;

    [Header("Icon")]
    [SerializeField] private Sprite leftButtonIcon;
    [SerializeField] private Sprite rightButtonIcon;

    [SerializeField] private Button leftBtn;
    [SerializeField] private Button rightBtn;

    [SerializeField] private Button heartButton;
    [SerializeField] private Button crossButton;

    [SerializeField] private int selectedCardIndex;
    [SerializeField]
    private int currentIndex = 0;
    
    private void ClickAccept()
    {
        selectedCardIndex = currentIndex;
    }

    private void ClickReject()
    {
        currentIndex += 1;
        currentIndex %= cardData.CardDataCollection.Length;
        cardObject.SetupCard(cardData.CardDataCollection[currentIndex]);
    }

    private void UpdateScreen()
    {
        if (currentScreenIndex > 1)
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
        currentScreenIndex +=1;
        UpdateScreen();
    }

    private void ClickPrevious()
    {
        if (currentScreenIndex > 1)
        {
            currentScreenIndex -= 1;
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
        cardObject.SetupCard(cardData.CardDataCollection[currentIndex]);
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
