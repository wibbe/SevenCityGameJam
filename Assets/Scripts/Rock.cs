using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float rotationSpeed = 0.4f;

    private Rigidbody m_rigidbody = null;

    private void Start()
    {
        transform.rotation = Random.rotationUniform;

        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.AddTorque(new Vector3(Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed), Random.Range(-rotationSpeed, rotationSpeed)));
    }
}
