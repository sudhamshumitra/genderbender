using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SequenceData", menuName = "ScriptableObjects/NarrativeSequence", order = 1)]

public class PageNumberScriptableObject : ScriptableObject
{
    public List<EPageType> narrativeSequence;
}