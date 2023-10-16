using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableContentEvent : MonoBehaviour, ISelectHandler
{
    public float overflow;
    public RectTransform contentRT;
    public void OnSelect(BaseEventData eventData)
    {
        RectTransform rt = GetComponent<RectTransform>();
        float bottom = overflow - rt.offsetMin.y;
        float top = contentRT.rect.height - overflow - rt.offsetMax.y;
        if (bottom < contentRT.anchoredPosition.y)
            contentRT.anchoredPosition = new Vector2(contentRT.anchoredPosition.x, bottom);
        else if (top > contentRT.anchoredPosition.y)
            contentRT.anchoredPosition = new Vector2(contentRT.anchoredPosition.x, top);
    }
}
