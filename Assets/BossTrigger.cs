using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
   BossManager bossManager;
    Collider bossTrigger;
    private void Awake()
    {
        bossManager = FindAnyObjectByType<BossManager>();
        bossTrigger = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Invoke("StartCutScene", 3.10f);
        bossTrigger.enabled = false;
    }
    private void StartCutScene() => bossManager.StartCutscene();
}
