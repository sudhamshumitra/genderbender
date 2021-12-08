using System;
using UnityEngine;

[Serializable]
public struct CardItem
{
    public string profileName;
    public Sprite coverSprite;
    public Vector4 coverOffset;
    public string coverText;
    public float aboutTextHeight;
    public string aboutText;
    public ImageItem[] basics;
    public ImageItem[] interests;
    public ProfileImage[] profileImages;
    public ImageItem[] spotifyItems;
    public Sprite[] instagramSprites;
    public ExperienceDataScriptableObject experienceData;
    public PromptData[] prompts;
}