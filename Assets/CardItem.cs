﻿using System;
using UnityEngine;

[Serializable]
public struct CardItem
{
    public string profileName;
    public Sprite coverSprite;
    public Vector4 coverOffset;
    public string coverText;
    public string aboutText;
    public ImageItem[] basicsAndInterests;
    public ProfileImage[] profileImages;
    public ImageItem[] spotifyItems;
    public Sprite[] instagramSprites;
    public ProfileImage[] changeSpritesInComplete;
    public Sprite[] ChangeBgSprites;
    public PromptData[] prompts;
}