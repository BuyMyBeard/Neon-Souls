using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeTryGetClose : MonoBehaviour
{
    Enemy enemy;
    [SerializeField] float closeIn;
    [SerializeField] float closeOut;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemy.inRangeInitEvent.AddListener(StartTryGetClose);
        enemy.inRangeExitEvent.AddListener(StopTryGetClose);
        enemy.closeInitEvent.AddListener(StartCheckIfStillClose);
        enemy.closeExitEvent.AddListener(StopCheckIfStillClose);
    }
    private void StartTryGetClose() => StartCoroutine(nameof(TryGetClose));
    private void StopTryGetClose() => StopCoroutine(nameof(TryGetClose));
    private void StartCheckIfStillClose() => StartCoroutine(nameof(CheckIfStillClose));
    private void StopCheckIfStillClose() => StopCoroutine(nameof(CheckIfStillClose));
    IEnumerator TryGetClose()
    {
        yield return new WaitUntil(() => enemy.DistanceFromPlayer < closeIn);
        enemy.ChangeMode(Enemy.ModeId.Close);
    }
    IEnumerator CheckIfStillClose()
    {
        yield return new WaitUntil(() => enemy.DistanceFromPlayer > closeOut);
        enemy.ChangeMode(Enemy.ModeId.InRange);
    }
}
