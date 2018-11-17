using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float initialForce = 100f;
    public float maxSpeed = 10f;

    private Rigidbody m_body = null;

    private void Start()
    {
    	m_body = GetComponent<Rigidbody>();    
    	m_body.AddForce(new Vector3(0f, initialForce, 0f));
    }

    private void FixedUpdate()
    {
        Vector3 velocity = m_body.velocity;
        velocity.Normalize();
        m_body.velocity = velocity * maxSpeed;
        m_body.AddForce(velocity * initialForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            other.GetComponent<Pickup>().target = transform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup") && Vector3.Distance(transform.position, other.transform.position) > 1.0f)
        {
            maxSpeed += 5f;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    	if (other.gameObject.CompareTag("Rock"))
        {
    		Debug.Log("Game Over");
    	}
    }
}
