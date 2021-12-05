using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BandScreen : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI topText;
    [SerializeField] private Transform originalContainer;
    [SerializeField] private RectTransform targetContainer;
    [SerializeField] private GameObject spotifyBandPrefab;

    private class Data
    {
        public List<SpotifyBandInfo> BandInfos;
    }

    private Data _data;

    private void ClearPreviousBands()
    {
        var previousBands = new List<GameObject>();
        for (var i = 0; i < targetContainer.childCount; i++)
        {
            if (targetContainer.GetChild(i).gameObject.name.Contains("SpotifyBandToPickPrefab"))
            {
                previousBands.Add(targetContainer.GetChild(i).gameObject);
            }
        }
        
        for (var i = 0; i < originalContainer.childCount; i++)
        {
            if (originalContainer.GetChild(i).gameObject.name.Contains("SpotifyBandToPickPrefab"))
            {
                previousBands.Add(originalContainer.GetChild(i).gameObject);
            }
        }
        foreach (var imageChild in previousBands)
        {
            DestroyImmediate(imageChild);
        }
    }
    
    private void ResetScreen()
    {
        ClearPreviousBands();
    }

    public void SetupBandScreen(string starterString, List<SpotifyBandInfo> bandData)
    {
        ClearPreviousBands();
        UpdateText(starterString);
        
        _data = new Data
        {
            BandInfos = bandData
        };

        foreach (var spotifyBandObject in bandData)
        {
            var bandObject = Instantiate(spotifyBandPrefab, originalContainer.transform).GetComponent<DragBandObject>();
            bandObject.SetupObject(targetContainer, this,_canvas , spotifyBandObject);
        }
    }
    
    public void UpdateText(string updatedText)
    {
        topText.text = updatedText;
    }
}