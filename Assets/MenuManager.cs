using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;
    EventSystem eventSystem;
    InputSystemUIInputModule inputModule;
    private void Awake()
    {
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
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
            eventSystem.SetSelectedGameObject(firstSelected);
        }
    }

    public void Play()
    {

    }
    public void Pause()
    {

    }
    public void Quit()
    {

    }
    public void Resume()
    {

    }
    public void OpenOptions()
    {

    }
    public void CloseOptions()
    {

    }
}
