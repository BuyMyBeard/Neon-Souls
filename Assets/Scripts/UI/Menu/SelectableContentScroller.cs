using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectableContentScroller : MonoBehaviour
{
    public float ContentHeight {  get; private set; }
    public float ViewportTop { get; private set; }
    public float ViewportBottom { get; private set; }
    public ScrollRect ScrollRect { get; private set; }
    IEnumerator Start()
    {
        yield return null;
        ScrollRect = GetComponentInParent<ScrollRect>();

        Vector3[] contentCorners = new Vector3[4];
        ScrollRect.content.GetWorldCorners(contentCorners);
        ContentHeight = contentCorners[1].y - contentCorners[0].y;

        Vector3[] viewportCorners = new Vector3[4];
        ScrollRect.GetComponent<RectTransform>().GetWorldCorners(viewportCorners); 
        ViewportBottom = viewportCorners[0].y;
        ViewportTop = viewportCorners[1].y;

        foreach (Selectable s in GetComponentsInChildren<Selectable>())
        {
            SelectableContentEvent component = s.AddComponent<SelectableContentEvent>();
            component.scs = this;
        }
    } 
}