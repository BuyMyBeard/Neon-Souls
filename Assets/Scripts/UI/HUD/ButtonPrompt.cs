using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField]Transform snapObject;
    CharacterController playerCharacter;
    ButtonPrompt buttonPrompt;
    public string promptMessage = "Interact";
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
public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPrompt;
    [SerializeField] GameObject[] display;
    //[SerializeField] float cooldown = 0.5f;
    Transform player;
    readonly List<Interactable> possiblePrompts = new();
    public Interactable currentPrompt;
    //bool onCooldown = false;

    void Awake()
    {
        // TODO: find player transform
        // player = FindObjectOfType<PlayerMove>().transform;
        HidePrompt();
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

}