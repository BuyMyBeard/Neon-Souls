using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrackScript : MonoBehaviour
{
    bool isActive = false;
    
    List<Transform> sawLaunchers = new();
    [SerializeField] int sawsPerSalvo = 4;
    [SerializeField] float spawnRate = 2f;
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
            LaunchSaws();
        }

    }
    void LaunchSaws()
    {
        foreach(Transform spawnPosition in GetLaunchPosition())
        {
            GameObject newSaw = Instantiate(saw, spawnPosition.position, spawnPosition.rotation);
            newSaw.transform.Rotate(0f, 0f, 90);
            newSaw.GetComponent<SawScript>().MoveSaw();
        }
    }
    List<Transform> GetLaunchPosition()
    {
        List<Transform> spawnPositions = new ();
        for(int i = 0; i < sawsPerSalvo; i++)
        {
            Transform currentSawLauncher = sawLaunchers[Random.Range(0, sawLaunchers.Count)];
            if(!spawnPositions.Contains(currentSawLauncher))
                spawnPositions.Add(currentSawLauncher);
        }
        return spawnPositions;
    }
}
