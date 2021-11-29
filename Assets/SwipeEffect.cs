using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeEffect : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    private Vector3 _initialPosition;
    private float _distanceMoved;
    private bool _swipeLeft;
    private Color _startColor;
    private bool _canStartDragging = true;
    [SerializeField]
    private float _cardRotation;
    
    private void Start()
    {
        _startColor = GetComponent<Image>().color;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition = new Vector2(transform.localPosition.x+eventData.delta.x,transform.localPosition.y);
        if ((transform.localPosition.x - _initialPosition.x) > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0,
                Mathf.Lerp(0, -_cardRotation, (_initialPosition.x + transform.localPosition.x) / (Screen.width / 2)));
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0,
                Mathf.Lerp(0, _cardRotation, (_initialPosition.x - transform.localPosition.x) / (Screen.width / 2)));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!_canStartDragging) return;
        _initialPosition = transform.localPosition;
    }

    private void ResetCard()
    {
        transform.localPosition = _initialPosition;
        transform.localEulerAngles = Vector3.zero;
        GetComponent<Image>().color = _startColor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _distanceMoved = Mathf.Abs(transform.localPosition.x - _initialPosition.x);
        if(_distanceMoved<0.4*Screen.width)
        {
            ResetCard();
        }
        else
        {
            if (transform.localPosition.x > _initialPosition.x)
            {
                _swipeLeft = false;
                
            }
            else
            {
                _swipeLeft = true;
            }
            StartCoroutine(MovedCard());
        }
    }
    
    private IEnumerator MovedCard()
    {
        float time = 0;
        while (GetComponent<Image>().color != new Color(_startColor.r,_startColor.g,_startColor.b, 0))
        {
            time += Time.deltaTime;
            if (_swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x-Screen.width,time),transform.localPosition.y,0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x+Screen.width,time),transform.localPosition.y,0);
            }
            GetComponent<Image>().color = new Color(_startColor.r,_startColor.g,_startColor.b,Mathf.SmoothStep(1,0,4*time));
            yield return null;
        }
        
        transform.SetAsFirstSibling();
        ResetCard();
        _canStartDragging = false;
    }
}
