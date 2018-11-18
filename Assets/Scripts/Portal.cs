using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject warpEffect = null;
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
        if (collision.gameObject.CompareTag("Player") && !gameManager.gameOver)
        {
            Instantiate(warpEffect, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Player>().graphics.SetActive(false);
            gameManager.EnterPortal();
        }
    }
}
