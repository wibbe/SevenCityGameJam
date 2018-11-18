using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float energyLevel = 20f;
    public float rotationSpeed = 0.4f;
    public GameObject collisionEffectPrefab = null;

    private Rigidbody m_rigidbody = null;


    private void Start()
    {
        transform.rotation = Random.rotationUniform;

        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.AddTorque(new Vector3(Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed)));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Edge") && collisionEffectPrefab != null)
        {
            Instantiate(collisionEffectPrefab, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
