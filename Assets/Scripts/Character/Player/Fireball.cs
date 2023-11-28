using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] Vector3 gravity;
    [SerializeField] int baseDamage = 100;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float AOE;
    [SerializeField] LayerMask layerOfCollision;
    public int damageScalingBonus = 0;
    Rigidbody rb;
    Explosion explosion;
    public bool thrown = false;
    bool Exploded = false;
    Stagger playerStagger;

    public int BaseDamage => baseDamage;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        explosion = GetComponent<Explosion>();
        playerStagger = GameObject.Find("Player").GetComponent<Stagger>();
    }
    private void FixedUpdate()
    {
        if (thrown)
            rb.AddForce(gravity, ForceMode.Acceleration);
        else if (!thrown && playerStagger.IsStaggered)
            Destroy(gameObject);
    }
    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (Exploded) return;

        Exploded = true;
        Collider[] ennemyColliders = Physics.OverlapSphere(collider.transform.position, AOE, layerOfCollision);
        
        HashSet<EnemyHealth> listUnique = new();
        foreach(Collider col in ennemyColliders)
        {
            EnemyHealth e = col.GetComponentInParent<EnemyHealth>();
            
            if(e != null)
                listUnique.Add(e);
        }
        
        foreach(EnemyHealth enemy in listUnique)
        {
            enemy.InflictDamage(baseDamage + damageScalingBonus);
        }

        /*Health opponentHealth = collider.GetComponentsInParent<Health>();
        if (opponentHealth != null)
            opponentHealth.InflictDamage(baseDamage + damageScalingBonus);*/

        // TODO: Particle explosion
        Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
