using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameManager gameManager = null;
    private Rigidbody targetRigidbody = null;
    private float force = 500.0f;
    private AudioSource audioSource = null;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GetComponentInParent<GameManager>();
    }

    void Update()
    {
        if (targetRigidbody != null)
        {
            targetRigidbody.AddForce((transform.position - targetRigidbody.position).normalized * force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            targetRigidbody = other.attachedRigidbody;
            audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0.0f;
            gameManager.EnterPortal();
        }
    }
}
