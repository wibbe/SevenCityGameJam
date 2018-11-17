using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject[] backgroundPrefabs = new GameObject[0];
    public GameObject pickup = null;



    [Space]
    public bool spawnRocks = true;
    public bool spawnPickups = true;
    public int rocksCount = 400;
    public int pickupCount = 40;
    public int backgroundCount = 300;
    public float gameAreaSize = 400f;
    public float minDistance = 30f;
    public float maxDistance = 100f;
    public int maxGravityWells = 5;
    
    [Space]
    public GameObject gameOverText = null;
    public GameObject victoryText = null;

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

        if (spawnRocks)
        {
            for (int i = 0; i < rocksCount; i++)
            {
                int type = UnityEngine.Random.Range(0, rockPrefabs.Length);
                Instantiate(rockPrefabs[type], new Vector3(UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, rockParent);
            }
        }

        for (int i = 0; i < backgroundCount; i++)
        {
            int type = UnityEngine.Random.Range(0, backgroundPrefabs.Length);
            Instantiate(backgroundPrefabs[type], new Vector3(UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), UnityEngine.Random.Range(minDistance, maxDistance)), Quaternion.identity, rockParent);
        }

        if (spawnPickups)
        {
            for (int i = 0; i < pickupCount; i++)
            {
                Instantiate(pickup, new Vector3(UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), UnityEngine.Random.Range(-gameAreaSize, gameAreaSize), 0f), Quaternion.identity, pickupParent);
            }
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

            if(pickupParent.childCount <= 0)
            {
                TextMeshProUGUI textMesh = victoryText.GetComponent<TextMeshProUGUI>();
                textMesh.text = "Victory\nScore: " + player.energy;
                victoryText.SetActive(true);
                End(3.0f);
            }
        }
    }

    public void EndGame()
    {
        gameOverText.SetActive(true);
        // End game
        StartCoroutine(End(2.0f));
        
    }

    private IEnumerator End(float endTime)
    {
        float time = 0.0f;
        while (time < endTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Menu");
    }
}
