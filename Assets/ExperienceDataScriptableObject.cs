using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceData", menuName = "ScriptableObjects/Experiences", order = 1)]
public class ExperienceDataScriptableObject : ScriptableObject
{
    public List<ChatElement> chatData1;
    public List<ChatElement> chatData2;
}