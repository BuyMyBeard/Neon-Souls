using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SelectableContentEvent : MonoBehaviour, ISelectHandler
{
    [HideInInspector] public SelectableContentScroller scs;
    const float TopMargin = 1.2f;
    const float BottomMargin = 1.2f;
    public void OnSelect(BaseEventData eventData)
    {
        GetComponentInParent<MenuManager>().OverrideFirstSelected(GetComponent<Selectable>());
        if (eventData.currentInputModule.IsPointerOverGameObject(Pointer.current.deviceId)) return;
        RectTransform rt = transform.GetComponent<RectTransform>();
        Vector3[] selectedCorners = new Vector3[4];
        rt.GetWorldCorners(selectedCorners);

        float selectedBottom = selectedCorners[0].y;
        float selectedTop = selectedCorners[1].y;

        float deltaBottom = scs.ViewportBottom - selectedBottom + BottomMargin;
        float deltaTop = scs.ViewportTop - selectedTop - TopMargin;
        if (deltaBottom > 0)
        {
            scs.ScrollRect.verticalNormalizedPosition = Mathf.Clamp01(scs.ScrollRect.verticalNormalizedPosition - deltaBottom / scs.ContentHeight * 3);
        }
        else if (deltaTop < 0)
        {
            scs.ScrollRect.verticalNormalizedPosition = Mathf.Clamp01(scs.ScrollRect.verticalNormalizedPosition - deltaTop / scs.ContentHeight * 3);
        }
    }
}
