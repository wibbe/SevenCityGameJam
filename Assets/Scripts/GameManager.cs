using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Space]
    public GameObject gameOverText = null;

    private void Start()
    {
        for (int i = 0; i < rocksCount; i++)
        {
            int type = UnityEngine.Random.Range(0, rockPrefabs.Length);
            Instantiate(rockPrefabs[type], new Vector3(UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, rockParent);
        }

        for (int i = 0; i < pickupCount; i++)
        {
            Instantiate(pickup, new Vector3(UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, pickupParent);
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

    public void EndGame()
    {
        // End game
        StartCoroutine(End(2.0f));
        
    }

    private IEnumerator End(float endTime)
    {
        float time = 0.0f;
        gameOverText.SetActive(true);
        while (time < endTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Menu");
    }
}
