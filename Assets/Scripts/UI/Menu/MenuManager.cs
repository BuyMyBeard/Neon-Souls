using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum SubMenus { None, Options, Credits }
public class MenuManager : MonoBehaviour
{
    [SerializeField] Selectable firstSelected;
    [SerializeField] GameObject optionsMenu;
    Selectable firstSelectedOverride;
    EventSystem eventSystem;
    InputSystemUIInputModule inputModule;
    Canvas menuDisplay;
    PlayerController playerController;
    public bool Paused { get; private set; } = false;
    public SubMenus CurrentSubMenu { get; private set; } = SubMenus.None;

    public bool IsInSubMenu { get => CurrentSubMenu != SubMenus.None; }
    public bool IsInMainMenu { get => SceneManager.GetActiveScene().buildIndex == 0; }
    private void Awake()
    {
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
        menuDisplay = GetComponentInChildren<Canvas>();
         if (!IsInMainMenu) playerController = FindObjectOfType<PlayerController>();
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
        inputModule.actionsAsset.FindActionMap("UI").FindAction("RestoreDefaults").started += RestoreDefaultsInput;
    }

    private void OnDisable()
    {
        inputModule.actionsAsset.Disable();
        inputModule.move.ToInputAction().performed -= MovePerformed;
        inputModule.leftClick.ToInputAction().performed -= MousePerformed;
        inputModule.point.ToInputAction().performed -= MousePerformed;
        inputModule.scrollWheel.ToInputAction().performed -= MousePerformed;
        inputModule.cancel.ToInputAction().performed -= BackInput;
        inputModule.actionsAsset.FindActionMap("UI").FindAction("RestoreDefaults").started -= RestoreDefaultsInput;
    }
    private void RestoreDefaultsInput(InputAction.CallbackContext obj)
    {
        if (CurrentSubMenu == SubMenus.Options)
            GetComponentInChildren<Settings>().ResetValues();
    }

    private void BackInput(InputAction.CallbackContext obj)
    {
        GoBack();
    }
    public void GoBack()
    {
            StartCoroutine(Back());
    }
    IEnumerator Back()
    {
        yield return null;
        if (!IsInSubMenu && !IsInMainMenu)
        {
            Resume();
        }
        else
        {
            ResetOverride();
            optionsMenu.SetActive(false);
            CurrentSubMenu = SubMenus.None;
        }
    }

    private void MousePerformed(InputAction.CallbackContext obj)
    {
        StartCoroutine(ResetSelected());
    }
    private void MovePerformed(InputAction.CallbackContext obj)
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
        Paused = true;
        Time.timeScale = 0;
        menuDisplay.gameObject.SetActive(true);
        playerController.SwitchToUI();
    }
    public void Resume()
    {
        if (IsInSubMenu)
        {
            GoBack();
            return;
        }
        Paused = false;
        Time.timeScale = 1;
        menuDisplay.gameObject.SetActive(false);
        playerController.SwitchToPlayerControls();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        FindObjectOfType<PlayerInput>().gameObject.SetActive(false);
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
