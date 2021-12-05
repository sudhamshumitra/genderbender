using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralImageScreenBehaviour : MonoBehaviour
{
    public class Data
    {
        public ProfileImage[] sprites;

        public Data(ProfileImage[] spriteCollection)
        {
            sprites = spriteCollection;
        }
    }
    private Data _data;

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;
    [SerializeField] private float fadeinTime = 0.4f;
    private Coroutine fadeinroutine;
    
    private void TextFadein()
    {
        _text.color = new Color(0, 0, 0, 0);
        _text.text = _data.sprites[currentImageIndex].profileText;
        fadeinroutine = StartCoroutine(FadeTextToFullAlpha(fadeinTime, _text));
    }

    private void ClearFadeIn()
    {
        if (fadeinroutine != null)
        {
            StopCoroutine(fadeinroutine);
        }
    }
    
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    
    public void SetupGeneralImageScreen(Data data)
    {
        _data = data;
        currentImageIndex = 0;
        UpdateScreen();
    }

    public void NextImage()
    {
        currentImageIndex++;
        currentImageIndex %= _data.sprites.Length;
        UpdateImage();
    }

    private int currentImageIndex = 0;
    
    public void PreviousImage()
    {
        currentImageIndex--;
        if (currentImageIndex < 0) currentImageIndex += _data.sprites.Length;
        UpdateImage();
    }
    private void StartScreen()
    {
        currentImageIndex = 0;
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        
        ClearFadeIn();
      
        UpdateImage();
    }

    void UpdateImage()
    {
        TextFadein();
        var imageItem = _data.sprites[currentImageIndex];
        _image.sprite = imageItem.content;
        _image.rectTransform.sizeDelta = new Vector2(imageItem.xywh.z, imageItem.xywh.w);
        _image.rectTransform.localPosition = new Vector2(imageItem.xywh.x, imageItem.xywh.y);
    }
    
    private void Start()
    {
        StartScreen();
    }
}
