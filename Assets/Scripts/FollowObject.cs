using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject: MonoBehaviour
{
    public Transform target = null;
    public float dragTime = 2f;


    private void Update()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, target.position, dragTime * Time.deltaTime);
        newPos.z = -20f;
        transform.position = newPos;
    }
}
