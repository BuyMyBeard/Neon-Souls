using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : DisplayBar
{
    public GameObject trackedEnemy; //TODO: Change to abstract class from which all ennemies inherit
    [SerializeField] Vector3 offset;
    Camera cam;
    Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        damageValue = GetComponentInChildren<TextMeshProUGUI>();
        if (trackedEnemy == null)
            gameObject.SetActive(false);
    }

    // Followed this implementation: https://gist.github.com/snlehton/27d2aa9591588fdacf75c8ab65bfb5f4
    // LateUpdate to track ennemies after they moved in Update
    private void LateUpdate()
    {
        var rt = GetComponent<RectTransform>();
        RectTransform parent = (RectTransform)rt.parent;
        var vp = cam.WorldToViewportPoint(trackedEnemy.transform.position);
        var sp = canvas.worldCamera.ViewportToScreenPoint(vp);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, sp, canvas.worldCamera, out Vector3 worldPoint);
        rt.position = worldPoint + offset;
    }
}