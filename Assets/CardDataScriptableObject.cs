using UnityEngine;

[CreateAssetMenu(fileName = "ChatProfile", menuName = "ScriptableObjects/Profiles", order = 1)]
public class CardDataScriptableObject : ScriptableObject
{
    public bool IsSamplingExperienceData;
    public CardItem[] CardDataCollection;
}