using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject: MonoBehaviour
{
    public Transform target = null;
    public float dragTime = 2f;

    private float z = 0f;

    private void Start()
    {
        z = transform.position.z;
    }

    private void Update()
    {
        
        Vector3 newPos = Vector3.Lerp(transform.position, target.position, dragTime * Time.deltaTime);
        newPos.z = z;
        transform.position = newPos;

        //Vector3 newRotation = Vector3.Lerp(transform.eulerAngles, target.eulerAngles, dragTime * Time.deltaTime);
        Debug.Log(Vector3.Angle(target.GetComponent<Rigidbody>().velocity.normalized, Vector3.up));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Vector3.Angle(target.GetComponent<Rigidbody>().velocity.normalized, Vector3.up)));
    }
}
