using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float initialForce = 100f;
    public float maxSpeed = 10f;

    private Rigidbody m_body = null;
    private Dictionary<GameObject, float> trappedPickups = new Dictionary<GameObject, float>();

    private void Start()
    {
    	m_body = GetComponent<Rigidbody>();    
    	m_body.AddForce(new Vector3(0f, initialForce, 0f));
    }

    private void FixedUpdate()
    {
        Vector3 velocity = m_body.velocity;
        velocity.Normalize();
        m_body.velocity = velocity * maxSpeed;
        m_body.AddForce(velocity * initialForce);

        //// Pull pickups
        //foreach (var pickupPair in trappedPickups)
        //{
        //    pickupPair.Key.transform.position = Vector3.Lerp(pickupPair.Key.transform.position, transform.position, 2.0f - pickupPair.Value);
        //    pickupPair.Value -= Time.deltaTime;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.tag == "Pickup")
        {
            if (!trappedPickups.ContainsKey(other.gameObject))
                trappedPickups.Add(other.gameObject, 2.0f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    	if (other.gameObject.CompareTag("Rock"))
    	{
    		Debug.Log("Game Over");
    	}
    }
}
