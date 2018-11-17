﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Skillster.Animation;

public class GameManager : MonoBehaviour
{
    public int seed = 7463883;

    [Space]
    public Camera mainCamera = null;
    public Player player = null;
    public GravityWell gravityWellPrefab = null;
    public Transform portalPrefab = null;
    public Transform gravityWellParent = null;
    public Transform rockParent = null;
    public Transform pickupParent = null;

    [Space]
    public GameObject pickup = null;
    public GameObject[] rockPrefabs = new GameObject[0];
    public GameObject[] backgroundPrefabs = new GameObject[0];


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
    public float energySuccessFaction = 0.6f;
    
    [Space]
    public GameObject gameOverText = null;
    public GameObject victoryText = null;
    public RectTransform energyLevel = null;
    public RectTransform energyLeftInLevel = null;
    public RectTransform winLevel = null;
    public CanvasGroup pauseMenu = null;


    private Queue<int> m_freeGravityWells = new Queue<int>();
    private bool m_pauseMenuVisible = false;
    private bool m_animatingPauseMenu = false;
    private bool m_inputEnabled = true;

    public float maxEnergy { get; private set; }
    public float energyLeft { get; private set; }
    public float successEnergy {  get { return maxEnergy * energySuccessFaction; } }



    public void RemoveWell(int shaderUniform)
    {
        Shader.SetGlobalVector(shaderUniform, new Vector4(0f, 0f, 0f, -1f));
        m_freeGravityWells.Enqueue(shaderUniform);
    }

    public void RemoveEnergyLeft(float amount)
    {
        energyLeft -= amount;
    }

    public void OnContinue()
    {
        if (m_pauseMenuVisible && !m_animatingPauseMenu)
            ContinueGame();
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
        UnityEngine.Random.InitState(seed);

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

        // Starting energy
        maxEnergy = player.energy;

        Pickup[] pickups = FindObjectsOfType<Pickup>();
        for (int i = 0; i < pickups.Length; i++)
        {
            if (pickups[i].transform.parent != pickupParent)
                pickups[i].transform.SetParent(pickupParent, true);

            maxEnergy += pickups[i].energyLevel;
        }

        energyLeft = maxEnergy - player.energy;

        winLevel.anchoredPosition = new Vector2(600f * energySuccessFaction, 0f);

        Debug.LogFormat("Max energy {0} from {1} pickups", maxEnergy, pickups.Length);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!m_pauseMenuVisible && !m_animatingPauseMenu)
            {
                PauseGame();
            }
            else if (m_pauseMenuVisible && !m_animatingPauseMenu)
            {
                ContinueGame();
            }
        }

        if (m_inputEnabled && Input.GetMouseButtonUp(0) && m_freeGravityWells.Count > 0)
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

        if (!victoryText.activeSelf && pickupParent.childCount <= 0)
        {
            TextMeshProUGUI textMesh = victoryText.GetComponent<TextMeshProUGUI>();
            textMesh.text = "Victory\nScore: " + player.energy;
            victoryText.SetActive(true);
            player.PlayVictorySound();
            StartCoroutine(End(2.0f));
        }

        energyLevel.sizeDelta = new Vector2((player.energy / maxEnergy) * 600f, 10f);
        energyLeftInLevel.sizeDelta = new Vector2(((player.energy + energyLeft) / maxEnergy) * 600f, 10f);
    }

    private void PauseGame()
    {
        m_pauseMenuVisible = true;
        m_animatingPauseMenu = true;
        m_inputEnabled = false;
        pauseMenu.gameObject.SetActive(true);
        pauseMenu.alpha = 0f;

        Tween.Callback(1f, 0f, 0.4f, (float t) => { Time.timeScale = t; });

        Tween.Track().Alpha(pauseMenu, 1f, 0.4f).Action(() =>
        {
            m_animatingPauseMenu = false;
        });
    }

    private void ContinueGame()
    {
        Tween.Track().Alpha(pauseMenu, 0f, 0.4f).Action(() =>
        {
            m_animatingPauseMenu = false;
            m_pauseMenuVisible = false;
            pauseMenu.gameObject.SetActive(false);

            m_inputEnabled = true;
        });

        Tween.Track().Delay(0.2f).Callback(0f, 1f, 0.4f, (float t) => { Time.timeScale = t; });
    }

    public void EndGame()
    {
        gameOverText.SetActive(true);
        player.PlayDefeatSound();
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
