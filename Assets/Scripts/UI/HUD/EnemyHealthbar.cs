using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : DisplayBar
{
    public Transform trackedEnemy;
    [SerializeField] Vector3 offset;
    [SerializeField] RectTransform indicator;
    Camera cam;
    Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        damageValue = GetComponentInChildren<TextMeshProUGUI>(); 
    }
    private void Start()
    {
        Hide();
    }

    // Followed this implementation: https://gist.github.com/snlehton/27d2aa9591588fdacf75c8ab65bfb5f4
    // LateUpdate to track ennemies after they moved in Update
    private void LateUpdate()
    {
        if (hidden) return;
        var rt = GetComponent<RectTransform>();
        RectTransform parent = (RectTransform)rt.parent;
        var vp = cam.WorldToViewportPoint(trackedEnemy.position + offset);
        var vp2 = cam.WorldToViewportPoint(trackedEnemy.position);
        var sp = canvas.worldCamera.ViewportToScreenPoint(vp);
        var sp2 = canvas.worldCamera.ViewportToScreenPoint(vp2);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, sp, canvas.worldCamera, out Vector3 worldPoint);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, sp2, canvas.worldCamera, out Vector3 worldPoint2);
        rt.position = worldPoint;
        indicator.position = worldPoint2;
    }

    public override void Hide()
    {
        base.Hide();
        indicator.gameObject.SetActive(false);
    }
    public override void Show()
    {
        base.Show();
        indicator.gameObject.SetActive(true);
    }
}