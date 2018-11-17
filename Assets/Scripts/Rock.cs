using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float tumbleSpeed = 0.4f;

    private Quaternion m_rotation;

    private void Start()
    {
        m_rotation = Random.rotationUniform;
    }

    private void Update()
    {
        //transform.rotation = transform.rotation * m_rotation * tumbleSpeed * Time.deltaTime;
    }
}
