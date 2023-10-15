using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;
    public GameObject firstSelectedOverride;
    EventSystem eventSystem;
    InputSystemUIInputModule inputModule;
    Canvas menuDisplay;
    private void Awake()
    {
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
        menuDisplay = GetComponentInChildren<Canvas>();
    }
    private void OnEnable()
    {
        inputModule.move.ToInputAction().performed += MovePerformed;
        inputModule.leftClick.ToInputAction().performed += ClickPerformed;
    }
    private void OnDisable()
    {
        inputModule.move.ToInputAction().performed -= MovePerformed;
        inputModule.leftClick.ToInputAction().performed -= ClickPerformed;
    }
    private void ClickPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartCoroutine(Click_performed());
    }
    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartCoroutine(Move_performed());
    }
    private IEnumerator Click_performed()
    {
        yield return null;
        eventSystem.SetSelectedGameObject(null);
    }
    private IEnumerator Move_performed()
    {
        yield return null;
        if (eventSystem.currentSelectedGameObject == null)
        {
            if (firstSelectedOverride == null)
                eventSystem.SetSelectedGameObject(firstSelected);
            else
                eventSystem.SetSelectedGameObject(firstSelectedOverride);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenOptions()
    {

    }
    public void CloseOptions()
    {

    }
}
