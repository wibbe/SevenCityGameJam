using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera = null;
    public Player player = null;
    public GravityWell gravityWellPrefab = null;
    public Transform gravityWellParent = null;
    public GameObject stars = null;
    public int starCount = 400;
    public float gameAreaSize = 400f;


    private void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            Instantiate(stars, new Vector3(Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-gameAreaSize, gameAreaSize), Random.Range(-10f, -50f)), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clicked!");

            Vector3 spawnPosition = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
            spawnPosition.z = 0f;

            GravityWell well = Instantiate<GravityWell>(gravityWellPrefab, spawnPosition, Quaternion.identity, gravityWellParent);
        }
    }

}
