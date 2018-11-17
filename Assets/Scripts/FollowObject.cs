using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject: MonoBehaviour
{
    public Transform target = null;
    public float dragTime = 2f;
    public float leadTime = 1f;
    public float zHeight = -100f;

    private Rigidbody m_targetBody = null;

    private void Start()
    {
        m_targetBody = target.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 newTarget = target.position + m_targetBody.velocity * leadTime;
        Vector3 newPos = Vector3.Lerp(transform.position, newTarget, dragTime * Time.deltaTime);
        newPos.z = zHeight;
        transform.position = newPos;

        //Vector3 newRotation = Vector3.Lerp(transform.eulerAngles, target.eulerAngles, dragTime * Time.deltaTime);
        //Debug.Log(Vector3.Angle(target.GetComponent<Rigidbody>().velocity.normalized, Vector3.up));
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector3.Angle(target.GetComponent<Rigidbody>().velocity.normalized, Vector3.up)));
    }
}
