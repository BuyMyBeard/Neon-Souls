using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] Transform snapObject;
    CharacterController playerCharacter;
    ButtonPrompt buttonPrompt;
    public string promptMessage = "Interact";
    public abstract string animationTriggerName { get; }
    protected virtual void Awake()
    {
        buttonPrompt = FindObjectOfType<ButtonPrompt>(); 
        GetComponent<Collider>().isTrigger = true;
        playerCharacter = FindObjectOfType<CharacterController>();
    }
    protected void Prompt()
    {
        buttonPrompt.ProposePrompt(this);
    }
    protected void CancelPrompt()
    {
        buttonPrompt.CancelPrompt(this);
    }
    public virtual void Interact()
    {
        playerCharacter.transform.SetPositionAndRotation(snapObject.position,snapObject.rotation);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Prompt();
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        CancelPrompt();
    }
    private void OnDrawGizmos()
    {
        if (snapObject == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(snapObject.position, new Vector3(1,0,1));

        Gizmos.DrawRay(snapObject.position, snapObject.forward);
    }
}
public class ButtonPrompt : MonoBehaviour, IControlsChangedListener
{
    [SerializeField] TextMeshProUGUI textPrompt;
    [SerializeField] GameObject[] display;
    //[SerializeField] float cooldown = 0.5f;
    Transform player;
    readonly List<Interactable> possiblePrompts = new();
    [SerializeField] Image keyboardInteractIcon;
    [SerializeField] Image gamepadInteractIcon;
    public Interactable currentPrompt;
    //bool onCooldown = false;
    Settings settings;

    void Awake()
    {
        // TODO: find player transform
        // player = FindObjectOfType<PlayerMove>().transform;
        HidePrompt();
        settings = FindObjectOfType<Settings>(true);
    }
    public void Interact()
    {
        if (currentPrompt != null && Time.timeScale != 0)
        {
            currentPrompt.Interact();
            CancelPrompt(currentPrompt);
            currentPrompt = null;
            HidePrompt();
            //StartCoroutine(Cooldown());
        }
    }

    //IEnumerator Cooldown()
    //{
    //    onCooldown = true;
    //    yield return new WaitForSeconds(cooldown);
    //    onCooldown = false;
    //}
    void LateUpdate()
    {
        if (possiblePrompts.Count == 0)
        {
            if (currentPrompt != null)
            {
                currentPrompt = null;
                HidePrompt();
            }
            return;
        }
        if (possiblePrompts.Count == 1)
        {
            if (possiblePrompts.First() == currentPrompt)
                return;
            currentPrompt = possiblePrompts.First();
        }
        else
        {
            Interactable closestPrompt = possiblePrompts.First();
            float shortestDistance = (player.position - closestPrompt.transform.position).magnitude;
            for (int i = 1; i < possiblePrompts.Count; i++)
            {
                float distance = (player.position - possiblePrompts[i].transform.position).magnitude;
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPrompt = possiblePrompts[i];
                }
            }
            currentPrompt = closestPrompt;
        }
        ShowPrompt(currentPrompt.promptMessage);
    }
    public void ProposePrompt(Interactable interactable)
    {
        settings.InitRebindActionDisplay();
        if (!possiblePrompts.Contains(interactable))
            possiblePrompts.Add(interactable);
    }
    public void CancelPrompt(Interactable interactable)
    {
        possiblePrompts.Remove(interactable);
    }
    public void HidePrompt()
    {
        foreach (var c in display) c.SetActive(false);
    }
    public void ShowPrompt(string message)
    {
        textPrompt.SetText(message);
        foreach (var c in display) c.SetActive(true);
    }

    public void ControlsChanged(SupportedDevices device)
    {
        if (device == SupportedDevices.Keyboard)
        {
            keyboardInteractIcon.enabled = true;
            gamepadInteractIcon.enabled = false;
        }
        else
        {
            keyboardInteractIcon.enabled = false;
            gamepadInteractIcon.enabled = true;
        }
    }
}