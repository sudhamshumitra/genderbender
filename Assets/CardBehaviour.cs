using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class CardPreset
{
    public static readonly float[] cellsize =
    {
        414,
        276,
        207,
    };
}

public class CardBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject imageItemPrefab;
    [SerializeField] private GameObject bandItemPrefab;
    [SerializeField] private Image instagramImagePrefab;
 
    [SerializeField] private TextMeshProUGUI profileCoverText;
    [SerializeField] private Image profileCoverImage;
    [SerializeField] private TextMeshProUGUI aboutText;
    
    [SerializeField] private Transform basicsAndInterestParent1;
    [SerializeField] private Transform basicsAndInterestParent2;

    [SerializeField] private Image profile2ndImage;
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
        imageItemObject.GetComponent<RectTransform>().sizeDelta = new Vector2( 80 + imageText.preferredWidth, 45);
    }

    private void DestroyChildren(Transform parentTransform)
    {
        int childs = parentTransform.childCount;
        for (int i = childs - 1; i >= 0; i--) {
            GameObject.DestroyImmediate( parentTransform.GetChild( i ).gameObject );
        }
    }
    
    public void SetupCard(CardItem cardItem)
    {
        profileCoverText.text = cardItem.coverText;
        profileCoverImage.sprite = cardItem.coverSprite;

        // aboutText.autoSizeTextContainer = true;
        
        aboutText.text = cardItem.aboutText;
        Debug.Log("width" + aboutText.preferredWidth);
        Debug.Log("height" +  aboutText.preferredHeight);
        var aboutRect = aboutText.rectTransform.rect;
        var area = aboutText.preferredWidth * aboutText.preferredHeight;

        aboutText.rectTransform.sizeDelta = new Vector2(0, area / 684.0f);
        Debug.Log("final" + aboutText.preferredHeight);
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
            var targetBasicsAndInterestParent = basicsAndInterestParent1.childCount < 3
                ? basicsAndInterestParent1
                : basicsAndInterestParent2;
            var imageItem = Instantiate(imageItemPrefab, targetBasicsAndInterestParent);
            SetupImageItem(ref imageItem, cardBasicItem);
        }

        DestroyChildren(bandParent1);
        DestroyChildren(bandParent2);
        
        foreach (var cardBasicItem in cardItem.spotifyItems)
        {
            var targetSpotifyBandParent = bandParent1.childCount < 2
                ? bandParent1
                : bandParent2;
            var imageItem = Instantiate(bandItemPrefab, targetSpotifyBandParent);
            SetupImageItem(ref imageItem, cardBasicItem);
        }
        
        profile2ndImage.sprite = cardItem.sprite2;

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
            instaObject.GetComponentInChildren<Image>().sprite = instagramSprite;
        }

        var totalHeight = 0.0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            totalHeight += transform.GetChild(i).GetComponent<RectTransform>().rect.height;
        }
        Debug.Log("total height: " + totalHeight);

        var originalSizeDelta = this.transform.GetComponent<RectTransform>().sizeDelta;
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSizeDelta.x, totalHeight);
    }
}