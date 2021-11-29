using UnityEngine;

[CreateAssetMenu(fileName = "ChatWindow", menuName = "ScriptableObjects/ChatPresetsScriptableObject", order = 1)]
public class ChatPresetsScriptableObject : ScriptableObject
{
    public float MaxWidthOfMessageItem = 520.0f;
    
    public float MinWidthChatItem = 250.0f;
    public float MinHeightChatItem = 75.0f;
    public float ChatElementSpacing = 20f;
    public float ChatElementLeftPosX = 20f;
    public float ChatElementRightPosX = 810f;
    public float WidthPerCharacter = 20.0f;
    public float HeightPerLine = 60.0f;
    public int MaxCharactersInALine = 27;
    public float ChatStartYPosition = 20.0f;
    public float CornerRound = 15;
    public Color ownerChatColor;
    public Color otherChatColor;
    public Color ownerTextColor;
    public Color otherTextColor;

   
}