using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class CardPreset
{
    public static readonly float[] cellsize =
    {
        685/2,
        276,
        207,
    };
}

[Serializable]
public struct PromptData
{
    public string promptLine;
    public string promptAnswer;
    public int promptPosition;
}

public class CardBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject promptPrefab;
    [SerializeField] private GameObject imageItemPrefab;
    [SerializeField] private GameObject bandItemPrefab;
    [SerializeField] private Image instagramImagePrefab;
    [SerializeField] private GameObject spacingPrefab;
    [SerializeField] private GameObject profileImagePrefab;
    [SerializeField] private GameObject trailingSpace;
    
    [SerializeField] private GameObject spotifySpace;
    [SerializeField] private GameObject spotifyContainer;
    [SerializeField] private GameObject instagramContainer;
    [SerializeField] private GameObject instagramSpace;
    
    [SerializeField] private TextMeshProUGUI profileCoverText;
    [SerializeField] private Image profileCoverImage;
    [SerializeField] private TextMeshProUGUI aboutText;
    [SerializeField] private TextMeshProUGUI instagramHeader;
    
    [SerializeField] private Transform basicsAndInterestParent1;
    [SerializeField] private Transform basicsAndInterestParent2;

    [SerializeField] private Transform bandParent1;
    [SerializeField] private Transform bandParent2;
    [SerializeField] private Transform instagramGridParent;
    [SerializeField] private ChatPresetsScriptableObject chatPreset;

    private void SetupImageItem(ref GameObject imageItemObject, ImageItem imageItem)
    {
        imageItemObject.transform.Find("Mask").transform.GetChild(0).GetComponent<Image>().sprite = imageItem.bandImage;
        imageItemObject.GetComponentInChildren<TextMeshProUGUI>().text = imageItem.bandName;
        var imageText = imageItemObject.GetComponentInChildren<TextMeshProUGUI>();

        //imageItemObject.GetComponent<RectTransform>().sizeDelta = new Vector2(area / 45, 45);
        imageItemObject.GetComponent<RectTransform>().sizeDelta = new Vector2( (80 + imageText.preferredWidth) * 1.1f, 49.5f);
    }

    public static void DestroyChildren(Transform parentTransform)
    {
        int childs = parentTransform.childCount;
        for (int i = childs - 1; i >= 0; i--) {
            GameObject.DestroyImmediate( parentTransform.GetChild( i ).gameObject );
        }
    }

    private void PlacePrompts(CardItem cardItem)
    {
        void DestoryPreviousPrompts()
        {
            var allImageChildrens = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("PromptContainer"))
                {
                    allImageChildrens.Add(transform.GetChild(i).gameObject);
                }
            }

            foreach (var imageChild in allImageChildrens)
            {
                    DestroyImmediate(imageChild);
            }
        }

        DestoryPreviousPrompts();
        foreach (var promptUnit in cardItem.prompts)
        {
            var promptBehavior = Instantiate(promptPrefab, transform);
            promptBehavior.transform.GetChild(0).Find("line").GetComponent<TextMeshProUGUI>().text = promptUnit.promptLine;
            promptBehavior.transform.GetChild(0).Find("answer").GetComponent<TextMeshProUGUI>().text = promptUnit.promptAnswer;
            promptBehavior.transform.SetSiblingIndex(1 + 2 * (promptUnit.promptPosition - 1));
            var spaceBehavior = Instantiate(spacingPrefab, transform);
            spaceBehavior.name = "PromptContainer " + spaceBehavior.name;
            spaceBehavior.transform.SetSiblingIndex(2 + 2 * (promptUnit.promptPosition - 1));
        }
    }
    
    public void SetupCard(CardItem cardItem)
    {
        transform.localPosition = new Vector2(0, 0);
        profileCoverText.text = cardItem.coverText;
        profileCoverImage.sprite = cardItem.coverSprite;
        profileCoverImage.rectTransform.sizeDelta = new Vector2(cardItem.coverOffset.z, cardItem.coverOffset.w);
        profileCoverImage.transform.localPosition = new Vector2(cardItem.coverOffset.x, cardItem.coverOffset.y);

        // aboutText.autoSizeTextContainer = true;
        instagramHeader.text = cardItem.profileName + "'s Instagram";
        aboutText.text = cardItem.aboutText;
        var aboutRect = aboutText.rectTransform.rect;
        var area = aboutText.preferredWidth * aboutText.preferredHeight;

        aboutText.rectTransform.sizeDelta = new Vector2(0, area / 684.0f);
        aboutText.rectTransform.sizeDelta = new Vector2(0, aboutText.preferredHeight);
        var calculatedSize = aboutText.rectTransform.rect;
        var parent = aboutText.transform.parent;
        var originalSize = parent.GetComponent<RectTransform>().sizeDelta;
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSize.x, aboutText.preferredHeight);
        //
        var parentOriginalSize = parent.transform.parent.GetComponent<RectTransform>().sizeDelta;
        parent.transform.parent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(parentOriginalSize.x, aboutText.preferredHeight);

        DestroyChildren(basicsAndInterestParent1);
        DestroyChildren(basicsAndInterestParent2);
        
        foreach (var cardBasicItem in cardItem.basicsAndInterests)
        {
            if (cardBasicItem.bandImage != null)
            {
                var targetBasicsAndInterestParent = basicsAndInterestParent1.childCount < 2
                    ? basicsAndInterestParent1
                    : basicsAndInterestParent2;
                var imageItem = Instantiate(imageItemPrefab, targetBasicsAndInterestParent);
                SetupImageItem(ref imageItem, cardBasicItem);
            }
        }

        DestroyChildren(bandParent1);
        DestroyChildren(bandParent2);

        
        PlaceSpotify(cardItem);
        PlaceInstagram(cardItem);
        PlacePrompts(cardItem);
        PlaceProfileImages(cardItem);
        PlaceTrailingSpace();
        UpdateHeight();

    }

    [SerializeField]
    private List<GameObject> alreadySeenGO;
    private void UpdateHeight()
    {
        var totalHeight = 0.0f;
        alreadySeenGO = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (alreadySeenGO.Contains(transform.GetChild(i).gameObject)) break;
            alreadySeenGO.Add(transform.GetChild(i).gameObject);
            Debug.Log(transform.GetChild(i).gameObject);
                
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                if (transform.GetChild(i).parent == transform)
                {
                    totalHeight += transform.GetChild(i).GetComponent<RectTransform>().rect.height;
                }
            }
        }

        var originalSizeDelta = this.transform.GetComponent<RectTransform>().sizeDelta;
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSizeDelta.x, totalHeight);
    }
    
    private void PlaceInstagram(CardItem cardItem)
    {
        if (cardItem.instagramSprites.Length == 0)
        {
            instagramSpace.gameObject.SetActive(false);
            instagramContainer.gameObject.SetActive(false);
        }
        else
        {
            instagramSpace.gameObject.SetActive(true);
            instagramContainer.gameObject.SetActive(true);
            
            int instaGridSizeIndex = 0;
            if (cardItem.instagramSprites.Length > 4)
            {
                instaGridSizeIndex = 1;
            }

            if (cardItem.instagramSprites.Length > 6)
            {
                instaGridSizeIndex = 2;
            }

            instagramGridParent.GetComponent<GridLayoutGroup>().cellSize =
                new Vector2(CardPreset.cellsize[instaGridSizeIndex], 225);

            DestroyChildren(instagramGridParent);
            foreach (var instagramSprite in cardItem.instagramSprites)
            {
                var instaObject = Instantiate(instagramImagePrefab, instagramGridParent);
                instaObject.transform.GetChild(0).GetComponent<Image>().sprite = instagramSprite;
            }
        }
    }

    private void PlaceSpotify(CardItem cardItem)
    {
        if (cardItem.spotifyItems.Length == 0)
        {
            spotifySpace.gameObject.SetActive(false);
            spotifyContainer.gameObject.SetActive(false);
        }
        else
        {
            spotifySpace.gameObject.SetActive(true);
            spotifyContainer.gameObject.SetActive(true);
            foreach (var cardBasicItem in cardItem.spotifyItems)
            {
                var targetSpotifyBandParent = bandParent1.childCount < 2
                    ? bandParent1
                    : bandParent2;
                var imageItem = Instantiate(bandItemPrefab, targetSpotifyBandParent);
                SetupImageItem(ref imageItem, cardBasicItem);
            }
        }
    }

    private void PlaceTrailingSpace()
    {
        trailingSpace.transform.SetAsLastSibling();
    }
    
    private void PlaceProfileImages(CardItem cardItem)
    {
        void DestroyPreviousImages()
        {
            var allImageChildrens = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Contains("ProfileImagePrefab"))
                {
                    allImageChildrens.Add(transform.GetChild(i).gameObject);
                }
            }

            foreach (var imageChild in allImageChildrens)
            {
                DestroyImmediate(imageChild);
            }
            
        }

        DestroyPreviousImages();
        foreach (var profileImageUnit in cardItem.profileImages)
        {
            var profileImageBehaviour = Instantiate(profileImagePrefab, transform);
            var profImageContent = profileImageBehaviour.transform.GetChild(0).Find("content").GetComponent<Image>();
            var desiredRect = profileImageUnit.xywh;
            profImageContent.rectTransform.sizeDelta = new Vector2(desiredRect.z, desiredRect.w);
            profImageContent.transform.localPosition = new Vector2(desiredRect.x, desiredRect.y);
            //profImageContent.rectTransform.rect.Set(desiredRect.x, desiredRect.y, desiredRect.z, desiredRect.w);
            profileImageBehaviour.transform.GetChild(0).Find("content").GetComponent<Image>().sprite = profileImageUnit.content;
            profileImageBehaviour.transform.SetSiblingIndex(1 + 2 * (profileImageUnit.position - 1));
            
            var spaceBehavior = Instantiate(spacingPrefab, transform);
            spaceBehavior.name = "ProfileImagePrefab " + spaceBehavior.name;
            spaceBehavior.transform.SetSiblingIndex(2 + 2 * (profileImageUnit.position - 1));
        }
    }
}