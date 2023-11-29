using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    Slider slider;
    private IEnumerator Start()
    {
        slider = GetComponentInChildren<Slider>();
        yield return new WaitForSeconds(.5f);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;

        
        while (operation.progress >= 0.9f)
        {
            slider.value = operation.progress;
            yield return new WaitForEndOfFrame();
        }
        slider.value = operation.progress;

        yield return new WaitForSeconds(.5f);
        slider.value = 1;
        yield return new WaitForSeconds(.5f);
        operation.allowSceneActivation = true;
    }
}

