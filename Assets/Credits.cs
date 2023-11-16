using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    [SerializeField] float timeToReachEnd = 30;
    ScrollRect scrollRect;
    IEnumerator Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        yield return new WaitUntil(() => scrollRect.verticalNormalizedPosition <= 0);
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        scrollRect.verticalNormalizedPosition -= Time.deltaTime / timeToReachEnd;
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }

}
