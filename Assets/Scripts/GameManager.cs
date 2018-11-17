using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera = null;
    public Player player = null;
    public GravityWell gravityWellPrefab = null;
    public Transform gravityWellParent = null;
    public Transform rockParent = null;
    public Transform pickupParent = null;

    [Space]
    public GameObject[] rockPrefabs = new GameObject[0];
    public GameObject pickup = null;

    [Space]
    public int rocksCount = 400;
    public int pickupCount = 40;
    public float gameAreaSize = 400f;
    public float minDistance = 30f;
    public float maxDistance = 100f;
    public int maxGravityWells = 5;

    private Queue<int> m_freeGravityWells = new Queue<int>();



    public void RemoveWell(int shaderUniform)
    {
        Shader.SetGlobalVector(shaderUniform, new Vector4(0f, 0f, 0f, -1f));
        m_freeGravityWells.Enqueue(shaderUniform);
    }

    private void Start()
    {
        for (int i = 0; i < maxGravityWells; i++)
        {
            int shaderID = Shader.PropertyToID(string.Format("_GravityWell{0}", i));
            m_freeGravityWells.Enqueue(shaderID);
        }

        for (int i = 0; i < rocksCount; i++)
        {
            int type = Random.Range(0, rockPrefabs.Length);
            Instantiate(rockPrefabs[type], new Vector3(Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, rockParent);
        }

        for (int i = 0; i < pickupCount; i++)
        {
            Instantiate(pickup, new Vector3(Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, pickupParent);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && m_freeGravityWells.Count > 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 spawnPosition = ray.GetPoint(distance);
            spawnPosition.z = 0f;

            GravityWell well = Instantiate<GravityWell>(gravityWellPrefab, spawnPosition, Quaternion.identity, gravityWellParent);
            int shaderUniform = m_freeGravityWells.Dequeue();
            well.Register(this, shaderUniform);
        }
    }
}
