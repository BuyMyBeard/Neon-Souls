using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    bool isLocked = false;
    List<Enemy> enemiesInSight = new ();
    Transform player;
    Transform camFollowTarget;
    [SerializeField] GameObject capsule;
    public void AddToLockOnList(Enemy enemy)
    {
        enemiesInSight.Add(enemy); 
    }
    public void RemoveFromLockOnList(Enemy enemy)
    {
        enemiesInSight.Remove(enemy);
    }
    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camFollowTarget = GameObject.FindGameObjectWithTag("FollowTarget").transform;
    }
    private void Update()
    {
        if (isLocked)
        {
            return;
        }

        
    }
    void OnLockOn()
    {
        if (isLocked)
        {
            return;
        }
        camFollowTarget.rotation = Quaternion.LookRotation(capsule.transform.forward, Vector3.up);
        Debug.Log(player.forward);
    }
}
