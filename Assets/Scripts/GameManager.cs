using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera = null;
    public Player player = null;
    public GravityWell gravityWellPrefab = null;
    public Transform gravityWellParent = null;
    public Transform starParent = null;
    public GameObject[] rockPrefabs = new GameObject[0];
    public int starCount = 400;
    public float gameAreaSize = 400f;
    public float minDistance = 30f;
    public float maxDistance = 100f;


    private void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            int type = Random.Range(0, rockPrefabs.Length);
            Instantiate(rockPrefabs[type], new Vector3(Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-gameAreaSize, gameAreaSize), Random.Range(minDistance, maxDistance)), Quaternion.identity, starParent);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Clicked!");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 spawnPosition = ray.GetPoint(distance);
            spawnPosition.z = 0f;

            GravityWell well = Instantiate<GravityWell>(gravityWellPrefab, spawnPosition, Quaternion.identity, gravityWellParent);
        }
    }

}
