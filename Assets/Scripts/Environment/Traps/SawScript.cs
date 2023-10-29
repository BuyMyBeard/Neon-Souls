using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    [SerializeField] float travelSpeed = 7f;
    [SerializeField] float travelDistance = 52f;
    [SerializeField] int sawDamage = 60;
    [SerializeField] float spinSpeed = 360;
    Vector3 initialPosition;
    Rigidbody rb;
    Transform model;
    Dictionary<int,float> healthList = new Dictionary<int, float>();
    [SerializeField]
    float sawDamageCooldown = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        model = transform.GetChild(0);
    }
    void Update()
    {
        model.Rotate(0, 0, -spinSpeed * Time.deltaTime, Space.Self);
        List<int> healths = new List<int>();
        foreach(KeyValuePair<int, float> entry in healthList)
        {
            if(entry.Value + sawDamageCooldown > Time.time)
                healths.Add(entry.Key);
        }
        foreach(int health in healths)
            healthList.Remove(health);

    }
    public IEnumerator MoveSaw(float yOffset)
    {
        yield return StartCoroutine(LerpPosition(yOffset));
        rb.AddForce(transform.forward * travelSpeed, ForceMode.VelocityChange);
        yield return StartCoroutine(SawCoroutine());
        yield return StartCoroutine(LerpPosition(-yOffset));
        Destroy(gameObject);
    }
    IEnumerator SawCoroutine()
    {
        yield return new WaitUntil(() => transform.position.x < initialPosition.x - travelDistance);
    }
    private void OnTriggerEnter(Collider other)
    {

        Health health = other.gameObject.GetComponentInParent<Health>();
        if(!healthList.ContainsKey(health.Id))
        {
            healthList.Add(health.Id, Time.time);
            health.InflictDamage(sawDamage);
        }
    }
    IEnumerator LerpPosition(float yOffset)
    {
        float timeElapsed = 0;
        float lerpDuration = 0.25f;
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = new(startPosition.x, startPosition.y - yOffset, startPosition.z);
        while (timeElapsed < lerpDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPosition;
    }
}
