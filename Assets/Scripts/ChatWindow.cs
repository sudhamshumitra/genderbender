using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class ChatWindow : MonoBehaviour
{
    [SerializeField] private ChatPresetsScriptableObject _chatPresetsScriptableObject;
    [SerializeField] private float lastChatHeight;
    [SerializeField] private float lastChatPositionY;
    [SerializeField] private TextMeshProUGUI ufui;
    
    private void Start()
    {
        lastChatPositionY = _chatPresetsScriptableObject.ChatStartYPosition;
    }

    public void PlayElement(ChatElement chatElement, GameObject typingIndicator)
    {
        AddElement(chatElement: chatElement, typingIndicator);
    }

    [SerializeField] private GameObject chatParent;
    [SerializeField] private ChatItem chatItem;

    public Vector2 GetNewElementPosition(bool isOwner)
    {
        return new Vector2(
            isOwner
                ? _chatPresetsScriptableObject.ChatElementRightPosX
                : _chatPresetsScriptableObject.ChatElementLeftPosX,
            (-_chatPresetsScriptableObject.ChatElementSpacing - lastChatHeight + lastChatPositionY));
    }

    private int GetNumberOfLinesInMessage(string message)
    {
        var messageLines = message.Split(new string[] {"<br>"}, StringSplitOptions.None);
        var numberOfLines = messageLines.Length - 1;
        foreach (var messageLine in messageLines)
        {
            if (messageLine.Length > _chatPresetsScriptableObject.MaxCharactersInALine)
            {
                numberOfLines++;
            }
        }

        return numberOfLines;
    }

    private int GetMaximumNumberOfCharactersInALine(string message)
    {
        var messageLines = message.Split(new string[] {"<br>"}, StringSplitOptions.None);
        var maximumNumberOfCharactersInALine = messageLines.Select(messageLine => messageLine.Length).Prepend(0).Max();
        return maximumNumberOfCharactersInALine;
    }

    private class ContentSize
    {
        public float width;
        public float height;

        public ContentSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }
    }
    
    
    private ContentSize GetContentSize(ChatElement chatElement)
    {
        var content = chatElement.GetContent();
        var width = GetMaximumNumberOfCharactersInALine(content) * _chatPresetsScriptableObject.WidthPerCharacter;
        var height = GetNumberOfLinesInMessage(content) * _chatPresetsScriptableObject.HeightPerLine;
        if (width > _chatPresetsScriptableObject.MaxWidthOfMessageItem)
            width = _chatPresetsScriptableObject.MaxWidthOfMessageItem;
        if (width < _chatPresetsScriptableObject.MinWidthChatItem)
            width = _chatPresetsScriptableObject.MinWidthChatItem;
        if (height < _chatPresetsScriptableObject.MinHeightChatItem)
            height = _chatPresetsScriptableObject.MinHeightChatItem;

        return new ContentSize(width, height);
    }

    public void SetProceduralUICorners(bool isOwner, FreeModifier freeModifier)
    {
        var upperLeft = _chatPresetsScriptableObject.CornerRound;
        var upperRight = _chatPresetsScriptableObject.CornerRound;
        var lowerLeft = _chatPresetsScriptableObject.CornerRound;
        var lowerRight = _chatPresetsScriptableObject.CornerRound;
        
        if (isOwner)
        {
            upperRight = 0;
        }
        else
        {
            upperLeft = 0;
        }
        
        freeModifier.Radius = new Vector4(upperLeft, upperRight, lowerLeft, lowerRight);
    }
    
    private void AddElement(ChatElement chatElement, GameObject typingIndicator)
    {
        var contentSize = GetContentSize(chatElement);
        var newChatElement = Instantiate(chatItem, chatParent.transform, true);
        var chatRect = newChatElement.GetComponent<RectTransform>();

        SetProceduralUICorners(chatElement.GetIsUserOwnedChat(), newChatElement.GetComponent<FreeModifier>());
            
        chatRect.pivot = chatElement.GetIsUserOwnedChat() ? new Vector2(1, 1) : new Vector2(0, 1);
        chatRect.GetComponent<ProceduralImage>().color =
            chatElement.GetIsUserOwnedChat()
                ? _chatPresetsScriptableObject.ownerChatColor
                : _chatPresetsScriptableObject.otherChatColor;
        chatRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentSize.width);
        chatRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentSize.height);

        newChatElement.SetSize(contentSize.width - 30, contentSize.height - 15);
        newChatElement.SetText(chatElement.GetContent());

        var chatNewPosition = GetNewElementPosition(chatElement.GetIsUserOwnedChat());
        var position = chatRect.position;
        position.Set(chatNewPosition.x, chatNewPosition.y, position.z);
        chatRect.anchoredPosition = position;

        lastChatPositionY = chatNewPosition.y;
        lastChatHeight = contentSize.height;
    }
}