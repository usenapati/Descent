using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public Transform vfxHit;
    public Transform vfxNoHit;
    private Rigidbody bulletRigidBody;
    public float projectileSpeed = 32.0f;
    public float damage = 10.0f;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigidBody.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect what it hits
        if (other.GetComponent<AILocomotion>() != null)
        {
            //Damage Enemy
            other.GetComponent<AILocomotion>().TakeDamage(damage);
            Instantiate(vfxHit, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(vfxNoHit, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
