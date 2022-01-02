using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    public float ragdollForce = 3000f;
    public float maxHealth;
    public HealthBar healthBar;


    private float currHealth;
    private Rigidbody[] rigRigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        rigRigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        currHealth = maxHealth;
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Weapon") && other.relativeVelocity.magnitude > 2f)
        {
            print("Collision Entered with Ragdoll");
            float damageDealt = other.relativeVelocity.magnitude * other.rigidbody.mass;
            TakeDamage(damageDealt);

            if (currHealth <= 0f)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                EnableRagdoll();

                Vector3 hitVector = (other.transform.position - transform.position).normalized;

                foreach(Rigidbody rb in rigRigidbodies)
                {
                    rb.AddForce(-hitVector * other.rigidbody.mass * ragdollForce);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        healthBar.UpdateHealth(currHealth / maxHealth);
    }

    private void DisableRagdoll()
    {
        foreach(Rigidbody rb in rigRigidbodies)
        {
            rb.isKinematic = true;
        }
    }
    
    private void EnableRagdoll()
    {
        foreach(Rigidbody rb in rigRigidbodies)
        {
            rb.isKinematic = false;
        }
    }


}