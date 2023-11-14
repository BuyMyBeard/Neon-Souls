using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class MeteorAttack : MonoBehaviour
{
    [SerializeField] float hoverTime = 3;
    [SerializeField] GameObject cracksDecal;
    [SerializeField] GameObject shadowDecal;
    Enemy enemy;
    EnemyAnimationEvents enemyAnimationEvents;
    Animator animator;
    NavMeshAgent agent;
    GameObject model;
    new Collider collider;
    public bool SkipThisFrameRootMotion { get; private set; } = false;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        model = enemy.transform.GetChild(0).gameObject;
        collider = GetComponent<Collider>();
    }

    [ContextMenu("Start Attack")]
    public void StartAttack()
    {
        animator.SetTrigger("MeteorLaunch");
        enemyAnimationEvents.FreezeMovement();
        enemyAnimationEvents.FreezeRotation();
        enemyAnimationEvents.DisableActions();
        agent.enabled = false;
    }
    void ReachSky() => StartCoroutine(WaitInSky());
    void TakeOff() => collider.enabled = false;
    void SyncLocation() => StartCoroutine(DoSyncLocation());

    IEnumerator DoSyncLocation()
    {
        SkipThisFrameRootMotion = true;
        transform.position = new Vector3(enemy.Target.position.x, transform.position.y, enemy.Target.position.z) + Vector3.forward * 0.001f + animator.deltaPosition;
        yield return null;
        SkipThisFrameRootMotion = false;
    }

    IEnumerator WaitInSky()
    {
        model.SetActive(false);
        GameObject shadow = Instantiate(shadowDecal);
        Transform parent = enemy.Target.GetComponentInParent<CharacterController>().transform;
        shadow.transform.position =parent.position;
        shadow.transform.parent = parent;
        yield return new WaitForSeconds(hoverTime);

        animator.SetTrigger("MeteorAttack");
        enemyAnimationEvents.UnFreezeRotation();
        model.SetActive(true);
    }

    void Land()
    {
        agent.enabled = true;
        collider.enabled = true;
        enemyAnimationEvents.FreezeRotation();
        Instantiate(cracksDecal).transform.position = transform.position;
    }
}
