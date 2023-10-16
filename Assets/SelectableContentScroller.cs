using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectableContentScroller : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        foreach (Selectable s in GetComponentsInChildren<Selectable>())
        {
            SelectableContentEvent component = s.AddComponent<SelectableContentEvent>();
            RectTransform rt = transform.parent.GetComponent<RectTransform>();
            component.contentRT = rt;
            component.overflow = (rt.rect.height - GetComponent<RectTransform>().rect.height) / 2f;
        }
    } 

}