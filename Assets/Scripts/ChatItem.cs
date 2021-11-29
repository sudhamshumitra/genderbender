using UnityEngine;

public class ChatItem : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI chatText;

    public void SetText(string message)
    {
        chatText.text = message;
    }
    
    public void SetSize(float width, float height)
    {
        var rectTransform = chatText.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }    
}
