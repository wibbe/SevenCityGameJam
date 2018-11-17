using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public float maxForce = 30f;
    public float life = 3f;
    public float radius = 10f;
    public AnimationCurve sizeAnimation;

    public SphereCollider m_collider;

    private float m_currentLife = 0f;
    private GameManager m_manager = null;
    private int m_shaderUniformID = -1;

    public void Register(GameManager manager, int shaderUniformID)
    {
        m_manager = manager;
        m_shaderUniformID = shaderUniformID;
    }

    private void OnDestroy()
    {
        m_manager.RemoveWell(m_shaderUniformID);
    }

    private void Update()
    {
        m_currentLife += Time.deltaTime;
        life -= Time.deltaTime;

        float scale = radius * sizeAnimation.Evaluate(m_currentLife / life);
        transform.localScale = new Vector3(scale, scale, scale);

        Vector3 pos = transform.position;
        Shader.SetGlobalVector(m_shaderUniformID, new Vector4(pos.x, pos.y, pos.z, scale));

        if (m_currentLife > life)
            Destroy(gameObject);
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            Vector3 dir = transform.position - other.attachedRigidbody.position;
            float distance = dir.magnitude;
            dir *= 1f / distance;

            distance = 1f - Mathf.Clamp01(distance / radius);
            distance *= distance;

            other.attachedRigidbody.AddForce(dir * maxForce * distance);
        }
    }
}
