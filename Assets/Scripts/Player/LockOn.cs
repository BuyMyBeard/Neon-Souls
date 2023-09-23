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
        player = GameObject.Find("PlayerPlaceHolder").transform; // a changer pour le nom du vrai model.
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
        else if (enemiesInSight.Count > 0)
        {
            Debug.Log("enemies is more than 0");
        }
        else
        {
            StartCoroutine(ResetCam());
           
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
}
