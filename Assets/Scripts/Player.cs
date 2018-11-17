using System;
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

    [Space]
    public GameManager gameManager = null;
    public CameraManager cameraManager = null;

    [Space]
    public AudioClip defeatClip = null;
    public AudioClip victoryClip = null;
    public AudioSource audioSource = null;
    public AudioSource flyAudioSource = null;

    [Space]
    public GameObject pickupEffectPrefab = null;
    public GameObject collisionEffectPrefab = null;
    public GameObject bounceEffectPrefab = null;
    
    private Rigidbody m_body = null;
    private float timeSinceLastCollision;
    private float timeSinceLastBounce;

    private void Start()
    {
    	m_body = GetComponent<Rigidbody>();    
    	m_body.AddForce(new Vector3(0f, initialForce, 0f));
    }

    private void Update()
    {
        timeSinceLastCollision += Time.deltaTime;
        timeSinceLastBounce += Time.deltaTime;
        if (energy > 0f)
        {
            energy -= energyDecay * Time.deltaTime;
        	float scale = Mathf.Lerp(minScale, maxScale, energy / gameManager.maxEnergy);
        	transform.localScale = new Vector3(scale, scale, scale);
        	MainModule ps = GetComponentInChildren<ParticleSystem>().main;
        	ps.startSizeMultiplier = scale;
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
        if (other.gameObject.CompareTag("Pickup") && Vector3.Distance(transform.position, other.transform.position) < Mathf.Lerp(minScale + 1f, maxScale + 1f, energy / gameManager.maxEnergy))
        {
            float energyLevel = other.gameObject.GetComponent<Pickup>().energyLevel;
            energy += energyLevel;
            gameManager.RemoveEnergyLeft(energyLevel);

            Destroy(other.gameObject);
            Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);

            cameraManager.Shake(0.3f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    	if (other.gameObject.CompareTag("Rock") && timeSinceLastCollision > 1.0f)
        {
            timeSinceLastCollision = 0.0f;
            energy -= other.gameObject.GetComponent<Rock>().energyLevel;
            Instantiate(collisionEffectPrefab, transform.position, Quaternion.identity);
            cameraManager.Shake(1.0f);
        }
        else if (other.gameObject.CompareTag("Edge") && timeSinceLastBounce > 0.3f)
        {
            timeSinceLastBounce = 0.0f;
            Instantiate(bounceEffectPrefab, transform.position, Quaternion.identity);
            cameraManager.Shake(0.6f);
        }
    }

    public void PlayDefeatSound()
    {
        PlaySound(defeatClip);
    }

    public void PlayVictorySound()
    {
        PlaySound(victoryClip);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
