using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBandObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool isInteractable = true;
    private Canvas canvas;
    private BandScreen bandScreen;
    private RectTransform targetRectTransform;
    
    [SerializeField]
    private Color originalColor;
    [SerializeField]
    private Color clickedColor;
    [SerializeField]
    private Color finalColor;
    [SerializeField]
    private Image bandImage;

    public void SetupObject(RectTransform targetRectTransform, BandScreen bandScreen, Canvas canvas, SpotifyBandInfo spotifyBandInfo)
    {
        this.isInteractable = true;
        this.canvas = canvas;
        this.bandScreen = bandScreen;
        this.targetRectTransform = targetRectTransform;
        this._spotifyBandInfo = spotifyBandInfo;
    }
    
    private SpotifyBandInfo _spotifyBandInfo;
    
    public void OnDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = this.transform.localPosition;
        bandImage.color = clickedColor;
    }

    bool IsPointInRT(Vector2 point, RectTransform rt)
    {
        // Get the rectangular bounding box of your UI element
        Rect rect = rt.rect;

        // Get the left, right, top, and bottom boundaries of the rect
        float leftSide = rt.anchoredPosition.x - rect.width / 2;
        float rightSide = rt.anchoredPosition.x + rect.width / 2;
        float topSide = rt.anchoredPosition.y + rect.height / 2;
        float bottomSide = rt.anchoredPosition.y - rect.height / 2;

        //Debug.Log(leftSide + ", " + rightSide + ", " + topSide + ", " + bottomSide);

        // Check to see if the point is in the calculated bounds
        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return true;
        }
        return false;
    }

    private Vector3 originalPosition;
    private void ResetPosition()
    {
        this.transform.localPosition = originalPosition;
    }
    

    public void OnEndDrag(PointerEventData eventData)
    {
        var isPlacedInFinal = RectTransformUtility.RectangleContainsScreenPoint(targetRectTransform, transform.position);
      
      if (isPlacedInFinal)
        {
            this.bandImage.color = finalColor;
            this.transform.parent = targetRectTransform.transform;
            bandScreen.UpdateText(_spotifyBandInfo.metaInfo);
            isInteractable = false;
        }
        else
        {
            this.bandImage.color = originalColor;
            this.ResetPosition();   
            
        }
    }
}