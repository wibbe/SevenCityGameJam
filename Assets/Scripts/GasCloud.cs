using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCloud : MonoBehaviour
{
    public float minScale = 1f;
    public float maxScale = 2f;
    public Material[] materials = new Material[0];

    public MeshRenderer renderer = null;


    private void Start()
    {
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));

        renderer.sharedMaterial = materials[Random.Range(0, materials.Length)];
    }

}
