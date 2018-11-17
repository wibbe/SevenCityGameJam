using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float lifeTime = 2.0f;
    
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
