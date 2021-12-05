using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GeneralScreenData
{
    public string originalPromptLine;
    public string originalPromptAnswer;
    public string originalTopText;
    public string updatedPromptText;
    public string updatedTopText;
}

[CreateAssetMenu(fileName = "ExperienceData", menuName = "ScriptableObjects/Experiences", order = 1)]
public class ExperienceDataScriptableObject : ScriptableObject
{
    public List<PageData> pageType;
    public List<string> directionScreens;
    public List<GeneralScreenData> GeneralScreenDataCollection;
    public List<GeneralImageScreenData> spriteChange;
    public List<ChatScreenData> chatData;
    public SpotifyScreenData spotifyData;
    public string swipeVideoName;
}