using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HearCombat : MonoBehaviour
{
    const float MaxSpawnListenRange = 40;
    [SerializeField] float hearRange;
    [SerializeField] float inquireStopDistance = 1;
    [SerializeField] float inquireTime = 4;

    NavMeshAgent agent;
    Enemy enemy;
    Patrol patrol;
    Health health;
    bool canPatrol;
    bool isListening = false;
    
    

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy.idleInitEvent.AddListener(StartDetectingPlayer);
        enemy.idleExitEvent.AddListener(StopDetectingPlayer);
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        canPatrol = TryGetComponent(out patrol);
        foreach (var characterHealth in FindObjectsOfType<Health>())
        {
            if (characterHealth is BossHealth || characterHealth == health) continue;
            else if (characterHealth is PlayerHealth) characterHealth.OnHitWithParam.AddListener(OnCharacterTakesDamage);
            else if (Vector3.Distance(health.transform.position, characterHealth.transform.position) > MaxSpawnListenRange) continue;
            characterHealth.OnHitWithParam.AddListener(OnCharacterTakesDamage);
        }
    }

    void OnCharacterTakesDamage(Health health)
    {
        Vector3 characterPosition = health is PlayerHealth ? health.transform.GetChild(0).position : health.transform.position;
        HearSound(characterPosition);
    }

    void HearSound(Vector3 position)
    {
        if (!isListening) return;
        if (Vector3.Distance(transform.position, position) > hearRange) return;

        NavMeshPath path = new();
        if (!(agent.CalculatePath(position, path) && path.status == NavMeshPathStatus.PathComplete)) return;

        if (canPatrol)
            patrol.StopPatrolling();
        agent.SetDestination(position);
        StopCoroutine(nameof(InquireSound));
        StartCoroutine(nameof(InquireSound));
    }
    IEnumerator InquireSound()
    {
        yield return null;
        yield return new WaitUntil(() => agent.remainingDistance < inquireStopDistance);
        yield return new WaitForSeconds(inquireTime);
        if (canPatrol)
            patrol.StartPatrolling();
    }

    void StartDetectingPlayer()
    {
        isListening = true;
    }


    void StopDetectingPlayer()
    {
        isListening = false;
        StopAllCoroutines();
    }
}
