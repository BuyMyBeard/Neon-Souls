using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TooCloseDetector : MonoBehaviour, IPlayerDetector
{
    const float TimeBetweenChecks = .2f;
    [SerializeField] float rangeToDetect = 3;
    [SerializeField] float timeToDetect = 2;
    NavMeshAgent agent;
    Enemy enemy;
    float timeElapsedInRange = 0;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemy.idleInitEvent.AddListener(StartDetectingPlayer);
        enemy.idleExitEvent.AddListener(StopDetectingPlayer);
        agent = GetComponent<NavMeshAgent>();
    }

    void StartDetectingPlayer()
    {
        StartCoroutine(nameof(DetectPlayer));
    }
    void StopDetectingPlayer()
    {
        StopCoroutine(nameof(DetectPlayer));
    }

    IEnumerator DetectPlayer()
    {
        timeElapsedInRange = 0;
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenChecks);
            if (Vector3.Distance(enemy.Target.transform.position, transform.position) < rangeToDetect)
            {
                timeElapsedInRange += TimeBetweenChecks;
                if (timeElapsedInRange >= timeToDetect)
                    enemy.ChangeMode(Enemy.ModeId.Close);
            }
            else
                timeElapsedInRange = 0;
        }
    }
}
