using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : DisplayBar
{
    private Transform trackedEnemy;
    public Transform TrackedEnemy
    {
        get => trackedEnemy;
        set
        {
            trackedEnemy = value;
            enemyHealth = value.GetComponentInParent<EnemyHealth>();
        }
    }
    EnemyHealth enemyHealth;
    [SerializeField] Vector3 offset;
    Camera cam;
    Canvas canvas;
    public RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        damageValue = GetComponentInChildren<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        Hide();
    }
    private void Start()
    {
    }

    // Followed this implementation: https://gist.github.com/snlehton/27d2aa9591588fdacf75c8ab65bfb5f4
    // LateUpdate to track ennemies after they moved in Update
    private void LateUpdate()
    {
        if (Hidden) return;
        RectTransform parent = (RectTransform)rt.parent;
        
        var vp = cam.WorldToViewportPoint(TrackedEnemy.position + offset);
        if (vp.z < 0) Hide();
        else Show();
        var sp = canvas.worldCamera.ViewportToScreenPoint(vp);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, sp, canvas.worldCamera, out Vector3 worldPoint);
        rt.position = worldPoint;
    }

    public override void Hide()
    {
        if (Hidden) return;
        base.Hide();
    }
    public override void Show(int damageTaken = 0)
    {
        if (!Hidden) return;
        Set(enemyHealth.CurrentHealth + damageTaken, enemyHealth.MaxHealth);
        base.Show();
    }
}