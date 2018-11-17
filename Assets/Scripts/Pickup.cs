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
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.AddTorque(new Vector3(Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed)));
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 2.0f - pullTime);
            pullTime -= Time.deltaTime;
        }
    }
}
