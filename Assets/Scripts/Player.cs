using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    public float initialForce = 100f;
    public float energy = 25.0f;
    public float energyDecay = 1.0f;
    public GameManager gameManager = null;

    private Rigidbody m_body = null;

    private void Start()
    {
    	m_body = GetComponent<Rigidbody>();    
    	m_body.AddForce(new Vector3(0f, initialForce, 0f));
    }

    private void Update()
    {
        energy -= energyDecay * Time.deltaTime;
        transform.localScale = new Vector3(energy / 10.0f, energy / 10.0f, energy / 10.0f);
        MainModule ps = GetComponentInChildren<ParticleSystem>().main;
        ps.startSizeMultiplier = energy / 10.0f;
        if (energy <= 0.0f)
        {
            gameManager.EndGame();
            // Explode or just disapear
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = m_body.velocity;
        velocity.Normalize();
        m_body.velocity = velocity * energy * 2.0f;
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
        if (other.gameObject.CompareTag("Pickup") && Vector3.Distance(transform.position, other.transform.position) < 2.0f)
        {
            Debug.Log(Vector3.Distance(transform.position, other.transform.position));
            energy += 5f;
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    	if (other.gameObject.CompareTag("Rock"))
        {
            gameManager.EndGame();
    		Debug.Log("Game Over");
    	}
    }
}
