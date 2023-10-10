using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrackScript : MonoBehaviour
{
    bool isActive = false;
    
    List<Transform> sawLaunchers = new();
    [SerializeField] int sawsPerSalvo = 4;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] float spawnXOffset = -2f;
    [SerializeField] float spawnYOffset = -2f;
    [SerializeField] GameObject saw;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in transform)
            sawLaunchers.Add(t);

    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 10) //Player layer
        {
            isActive = true;
            StartCoroutine(LaunchSawCoroutine());
            GetComponent<Collider>().enabled = false;
        }

    }
    IEnumerator LaunchSawCoroutine()
    {
        while (isActive)
        {
            LaunchSaws();
            yield return new WaitForSeconds(spawnRate);
        }
    }
    void LaunchSaws()
    {
        foreach(Transform launcherPosition in GetLaunchPosition())
        {
            Vector3 spawnPosition = launcherPosition.position;
            spawnPosition.x = spawnPosition.x + spawnXOffset;
            GameObject newSaw = Instantiate(saw, spawnPosition, launcherPosition.rotation, launcherPosition);
            newSaw.transform.Rotate(0f, 0f, 90);
            newSaw.GetComponent<SawScript>().MoveSaw();
        }
    }
    List<Transform> GetLaunchPosition()
    {
        List<Transform> spawnPositions = new ();
        for(int i = 0; spawnPositions.Count < sawsPerSalvo; i++)
        {
            Transform currentSawLauncher = sawLaunchers[Random.Range(0, sawLaunchers.Count)];
            if(!spawnPositions.Contains(currentSawLauncher))
                spawnPositions.Add(currentSawLauncher);
        }
        return spawnPositions;
    }
}
