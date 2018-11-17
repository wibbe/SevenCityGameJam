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
    public Transform pickupParent = null;
    public GameObject stars = null;
    public GameObject pickup = null;
    public int starCount = 400;
    public int pickupCount = 40;
    public float gameAreaSize = 400f;


    private void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            Instantiate(stars, new Vector3(Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-gameAreaSize, gameAreaSize), Random.Range(10f, 50f)), Quaternion.identity, starParent);
        }
        for (int i = 0; i < pickupCount; i++)
        {
            Instantiate(pickup, new Vector3(Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, pickupParent);
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
