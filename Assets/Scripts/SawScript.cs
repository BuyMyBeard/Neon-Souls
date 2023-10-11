using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    [SerializeField] float travelSpeed = 7f;
    [SerializeField] float travelDistance = 54f;
    [SerializeField] int sawDamage = 60;
    Vector3 initialPosition;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) // 10 == Player layer
            collision.gameObject.GetComponentInParent<Health>().InflictDamage(sawDamage);
    }
    IEnumerator LerpPosition(float yOffset)
    {
        float timeElapsed = 0;
        float lerpDuration = 0.25f;
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - yOffset, startPosition.z);
        while (timeElapsed < lerpDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPosition;
    }
}
