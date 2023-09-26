using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    bool isLocked = false;
    [SerializeField] float viewRadius = 90;
    [SerializeField, Range(0,360)] float viewAngle = 90;
    [SerializeField] LayerMask lockableMask;
    [SerializeField] LayerMask environmentMask;
    List<Lockable> enemiesInSight = new ();

    
    Transform player;
    Transform camFollowTarget;
    public void AddToLockOnList(Lockable lockable)
    {
        enemiesInSight.Add(lockable); 
    }
    public void RemoveFromLockOnList(Lockable lockable)
    {
        enemiesInSight.Remove(lockable);
    }
    public void Awake()
    {
        player = GameObject.Find("PlayerPlaceHolder").transform; // a changer pour le nom du vrai model.
        camFollowTarget = GameObject.FindGameObjectWithTag("FollowTarget").transform;
    }
    private void Update()
    {
        if (isLocked)
        {
            return;
        }
        FindEnemiesInSight();
        
    }
    void OnLockOn()
    {
        if (isLocked)
        {

            return;
        }
        else if (enemiesInSight.Count > 0)
        {

            Debug.Log("enemies are more than 0");
        }
        else
        {
            StartCoroutine(ResetCam());
           
        }
        
        
    }
    void FindEnemiesInSight()
    {
        enemiesInSight.Clear();
        Collider[] enemiesInViewRadius = Physics.OverlapSphere(camFollowTarget.position, viewRadius, lockableMask);
        for(int i = 0; i< enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInSight[i].transform;
            Vector3 directionToEnemy = (enemy.position - camFollowTarget.position).normalized;
            if(Vector3.Angle(camFollowTarget.forward, directionToEnemy) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(camFollowTarget.position, enemy.position);
                if(!Physics.Raycast(camFollowTarget.position, directionToEnemy, distanceToTarget, environmentMask))
                {
                    enemiesInSight.Add(enemy.GetComponent<Lockable>());
                    Debug.Log(enemy.name + "Added to list");
                }
            }
        }
    }

    /// <summary>
    /// Lerp fonctionne pas mais la camera snap au bon endroit. So, it kinda works.
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetCam()
    {
        float elapsedTime = 0f;
        float lerpTime = 0.5f;
        while (elapsedTime < lerpTime)
        {
            camFollowTarget.rotation = Quaternion.Lerp(player.rotation, camFollowTarget.rotation, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += camFollowTarget.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
