using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 movement;
    PlayerHealth playerHealth;

    public Transform target;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float homingTime;
    [SerializeField] float yDirDiffLimit;
    [SerializeField] int damage;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int staminaBlockCost = 15;

    ObjectPool pool;
    public Coroutine p_returnCoroutine;
    public Coroutine p_homingCoroutine;

    float originalYdir;
    float newYdir;
    [SerializeField] bool hasAlreadyHit = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerHealth>();
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnEnable()
    {
        pool = GetComponentInParent<ObjectPool>();
        movement = Vector3.forward;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        originalYdir = transform.rotation.eulerAngles.y;
        if (originalYdir > 180f) originalYdir = 360f - originalYdir;
        p_homingCoroutine = StartCoroutine(HomingCoroutine());
        hasAlreadyHit = false;
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(movement * speed * Time.deltaTime, Space.Self);
    }
    IEnumerator HomingCoroutine()
    {
        for (float elapsedTime = 0f; elapsedTime < homingTime; elapsedTime += Time.deltaTime)
        {
            var diff = target.position - transform.position;

            Debug.DrawLine(target.position, transform.position, Color.red);

            var dir = Quaternion.LookRotation(diff);
            newYdir = dir.eulerAngles.y;
            if (newYdir > 180f) newYdir = 360f - newYdir;
            if (Mathf.Abs(originalYdir - newYdir) > yDirDiffLimit)
                yield break;

            transform.rotation = Quaternion.Slerp(transform.rotation, dir, rotationSpeed);
            yield return null;
        }
        p_homingCoroutine = null;
    }

    void OnCollisionEnter(Collision other)
    {
        if (hasAlreadyHit) return;
        Health health = other.collider.GetComponentInParent<Health>();
        if (health != null)
            health.InflictBlockableDamage(damage, staminaBlockCost, transform);
        Despawn();
        hasAlreadyHit = true;
    }

    void Despawn()
    {
        pool.StopCoroutine(p_returnCoroutine);
        pool.ReturnObject(gameObject);
    }
}
