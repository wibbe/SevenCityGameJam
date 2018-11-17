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
        if(other.tag == "Pickup")
        {
            other.GetComponent<Pickup>().target = transform;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.name);
    	if (other.gameObject.CompareTag("Rock"))
        {
    		Debug.Log("Game Over");
    	}
        else if (other.gameObject.CompareTag("Pickup"))
        {
            maxSpeed += 5f;
            Destroy(other.gameObject);
        }
    }
}
