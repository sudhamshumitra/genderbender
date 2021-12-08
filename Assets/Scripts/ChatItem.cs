using UnityEngine;

public class ChatItem : MonoBehaviour
{
    [SerializeField]
    public TMPro.TextMeshProUGUI chatText;

    public void SetText(string message)
    {
        chatText.text = message;
    }
    
    public void SetSize(bool isOwned, float width, float height)
    {
        var rectTransform = chatText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
        // rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        // rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }    
}
