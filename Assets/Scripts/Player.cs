using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Rigidbody m_body = null;

    private void Start()
    {
    	m_body = GetComponent<Rigidbody>();    
    	m_body.AddForce(new Vector3(0f, 10f, 0f));
    }
}
