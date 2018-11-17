using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float rotationSpeed = 0;
    public float energyLevel;
    public Transform target = null;
    
    private Rigidbody m_rigidbody;
    private float pullTime = 2.0f;
    private float initialScale = 1f;
    

    private void Start()
    {
        initialScale = transform.localScale.x;
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.AddTorque(new Vector3(Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed)));
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 2.0f - pullTime);

            float scale = Mathf.Clamp01(pullTime / 2f) * initialScale;
            transform.localScale = new Vector3(scale, scale, scale);
            pullTime -= Time.deltaTime;
        }
    }
}
