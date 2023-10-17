using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum SubMenus { None, Options, Credits }
public class MenuManager : MonoBehaviour
{
    [SerializeField] Selectable firstSelected;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] bool pausable = false;
    Selectable firstSelectedOverride;
    EventSystem eventSystem;
    InputSystemUIInputModule inputModule;
    public SubMenus CurrentSubMenu { get; private set; } = SubMenus.None;


    public bool IsInSubMenu { get => CurrentSubMenu != SubMenus.None; }
    private void Awake()
    {
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
    }
    private void OnEnable()
    {
        optionsMenu.SetActive(false);
        inputModule.actionsAsset.Enable();
        inputModule.move.ToInputAction().performed += MovePerformed;
        inputModule.leftClick.ToInputAction().performed += MousePerformed;
        inputModule.point.ToInputAction().performed += MousePerformed;
        inputModule.scrollWheel.ToInputAction().performed += MousePerformed;
        inputModule.cancel.ToInputAction().performed += BackInput;
        inputModule.actionsAsset.FindActionMap("UI").FindAction("Pause").started += PauseInput;
    }


    private void OnDisable()
    {
        inputModule.actionsAsset.Disable();
        inputModule.move.ToInputAction().performed -= MovePerformed;
        inputModule.leftClick.ToInputAction().performed -= MousePerformed;
        inputModule.point.ToInputAction().performed -= MousePerformed;
        inputModule.scrollWheel.ToInputAction().performed -= MousePerformed;
        inputModule.cancel.ToInputAction().performed -= BackInput;
        inputModule.actionsAsset.FindActionMap("UI").FindAction("Pause").started -= PauseInput;
    }
    private void PauseInput(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
        if (pausable && !IsInSubMenu)
        {
            if (Time.timeScale == 0) Resume();
            else Pause();
        }
    }

    private void BackInput(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GoBack();
    }
    public void GoBack()
    {
        if (CurrentSubMenu != SubMenus.None)
            StartCoroutine(Back());
    }
    public IEnumerator Back()
    {
        yield return null;
        ResetOverride();
        optionsMenu.SetActive(false);
        CurrentSubMenu = SubMenus.None;
    }

    private void MousePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartCoroutine(ResetSelected());
    }
    private void MovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartCoroutine(MovePerformed());
    }
    private IEnumerator ResetSelected()
    {
        yield return null;
        eventSystem.SetSelectedGameObject(null);
    }
    private IEnumerator MovePerformed()
    {
        yield return null;
        if (eventSystem.currentSelectedGameObject == null)
        {
            if (firstSelectedOverride == null)
                eventSystem.SetSelectedGameObject(firstSelected.gameObject);
            else
                eventSystem.SetSelectedGameObject(firstSelectedOverride.gameObject);
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
    public void OverrideFirstSelected(Selectable gameObject)
    {
        firstSelectedOverride = gameObject;
        if (eventSystem.alreadySelecting) return;
        eventSystem.SetSelectedGameObject(gameObject.gameObject);
    }
    public void ResetOverride()
    {
        firstSelectedOverride = null;
        if (eventSystem.alreadySelecting) return;
        eventSystem.SetSelectedGameObject(firstSelected.gameObject);
    }

    public void SetSubMenu(SubMenus subMenu) => CurrentSubMenu = subMenu;
    public void SetSubMenuToOptions() => SetSubMenu(SubMenus.Options);
}
