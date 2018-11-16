using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public float maxForce = 30f;
    public float life = 3f;
    public float radius = 10f;

    public SphereCollider m_collider;


    private void Start()
    {
        m_collider.radius = radius;
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if (life < 0f)
            Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {
        //Debug.LogFormat("Stay {0}", other.name);
        Vector3 dir = transform.position - other.attachedRigidbody.position;
        float distance = dir.magnitude;
        dir *= 1f / distance;

        distance = 1f - Mathf.Clamp01(distance / radius);
        distance *= distance;

        other.attachedRigidbody.AddForce(dir * maxForce * distance);
    }
}
