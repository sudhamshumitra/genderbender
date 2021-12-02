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
    
    [SerializeField] private Image _image;
    
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
        UpdateImage();
    }

    void UpdateImage()
    {
        var imageItem = _data.sprites[currentImageIndex];
        _image.sprite = imageItem.content;
        _image.GetComponent<RectTransform>().sizeDelta = new Vector2(imageItem.xywh.z, imageItem.xywh.w);
        _image.GetComponent<RectTransform>().localPosition = new Vector2(imageItem.xywh.x, imageItem.xywh.y);
    }
    
    private void Start()
    {
        SetupGeneralImageScreen(new Data(null));
        StartScreen();
    }
}
