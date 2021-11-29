using System;
using UnityEngine;

[Serializable]
public struct ImageItem
{
    public Sprite bandImage;
    public string bandName;
}

[Serializable]
public struct CardItem
{
    public string profileName;
    public Sprite coverSprite;
    public string coverText;
    public string aboutText;
    public ImageItem[] basicsAndInterests;
    public Sprite sprite2;
    public ImageItem[] spotifyItems;
    public Sprite[] instagramSprites;
}

[CreateAssetMenu(fileName = "ChatProfile", menuName = "ScriptableObjects/Profiles", order = 1)]
public class CardDataScriptableObject : ScriptableObject
{
    public CardItem[] CardDataCollection;
}