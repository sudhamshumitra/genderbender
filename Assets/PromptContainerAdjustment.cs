using TMPro;
using UnityEngine;

public class PromptContainerAdjustment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI line;
    [SerializeField] private TextMeshProUGUI answer;

    public void Adjust()
    {
        var lineHeight = line.GetComponent<RectTransform>().rect.height;
        var answerHeight = answer.GetComponent<RectTransform>().rect.height;
        var desiredHeight = lineHeight + answerHeight + 80;

        answer.GetComponent<RectTransform>().localPosition =
            new Vector3(answer.GetComponent<RectTransform>().localPosition.x, -40 - lineHeight);
        
        transform.GetComponent<RectTransform>().sizeDelta =
            new Vector2(transform.GetComponent<RectTransform>().rect.width,desiredHeight);

        var parent = line.transform.parent;
        parent.GetComponent<RectTransform>().sizeDelta =
            new Vector2(parent.GetComponent<RectTransform>().rect.width, desiredHeight);
    }

    
    private float GetHeightAccordingToPreferredArea(ref TextMeshProUGUI textMeshProUGUI, float widthConstraint)
    {
        var preferredArea = textMeshProUGUI.preferredHeight * textMeshProUGUI.preferredWidth;
        return preferredArea / widthConstraint;
    }
    
}
