using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public float force = 30f;
    public float life = 3f;
    public float radius = 10f;
    public AnimationCurve sizeAnimation;

    public SphereCollider m_collider;

    private float m_currentLife = 0f;


    private void Start()
    {
        //m_collider.radius = 1.2f;
    }

    private void Update()
    {
        m_currentLife += Time.deltaTime;
        life -= Time.deltaTime;

        float scale = radius * sizeAnimation.Evaluate(m_currentLife / life);
        transform.localScale = new Vector3(scale, scale, scale);

        if (m_currentLife > life)
            Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            float playerEnergyFactor = Mathf.Max(other.GetComponent<Player>().energy * 1.5f / 25.0f, 0.5f);
            Debug.Log(playerEnergyFactor);
            Vector3 dir = transform.position - other.attachedRigidbody.position;
            float distance = dir.magnitude;
            dir *= 1f / distance;

            distance = 1f - Mathf.Clamp01(distance / radius);
            distance *= distance;

            other.attachedRigidbody.AddForce(dir * force * playerEnergyFactor * distance);
        }
    }
}
