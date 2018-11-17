using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    public float initialForce = 100f;
    public float energy = 25.0f;
    public float energyDecay = 1.0f;
    public float speed = 30.0f;
    public float minScale = 1f;
    public float maxScale = 3f;
    public GameManager gameManager = null;

    private Rigidbody m_body = null;

    private void Start()
    {
    	m_body = GetComponent<Rigidbody>();    
    	m_body.AddForce(new Vector3(0f, initialForce, 0f));
    }

    private void Update()
    {
        if (energy > 0f)
        {
            energy -= energyDecay * Time.deltaTime;
            float scale = Mathf.Lerp(minScale, maxScale, energy / gameManager.maxEnergy);
            transform.localScale = new Vector3(scale, scale, scale);
            MainModule ps = GetComponentInChildren<ParticleSystem>().main;
            ps.startSizeMultiplier = scale;
            if (energy <= 0.0f)
            {
                gameManager.EndGame();
                // Explode or just disapear
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 velocity = m_body.velocity;
        velocity.Normalize();
        m_body.velocity = velocity * speed;
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
        if (other.gameObject.CompareTag("Pickup") && Vector3.Distance(transform.position, other.transform.position) < Mathf.Max(1.0f, 4.0f * energy / 10.0f))
        {
            energy += other.gameObject.GetComponent<Pickup>().energyLevel;
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
