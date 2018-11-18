using System;
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
    public GameObject portal = null;
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
    public CanvasGroup gameOverMenu = null;
    public CanvasGroup gameDoneMenu = null;


    private Queue<int> m_freeGravityWells = new Queue<int>();
    private bool m_pauseMenuVisible = false;
    private bool m_gameOverMenuVisible = false;
    private bool m_animatingMenu = false;
    public bool inputEnabled = true;

    public float maxEnergy { get; private set; }
    public float energyLeft { get; private set; }
    public float successEnergy {  get { return maxEnergy * energySuccessFaction; } }
    public bool gameOver { get; private set; }
    public bool levelDone { get; private set; }


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
        if (m_pauseMenuVisible && !m_animatingMenu)
            ContinueGame();
    }

    public void OnNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
        //SceneManager.LoadScene(sceneIndex + 1); When we have more levels
        // Also check if it's the last level
    }

    public void OnRetry()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnQuit()
    {
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
    }

    private void Awake()
    {
        Time.timeScale = 1.0f;
        UnityEngine.Random.InitState(seed);
        gameOver = false;
        levelDone = false;
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
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if (!m_pauseMenuVisible && !m_animatingMenu)
            {
                PauseGame();
            }
            else if (m_pauseMenuVisible && !m_animatingMenu)
            {
                ContinueGame();
            }
        }

        if (!gameOver && inputEnabled && Input.GetMouseButtonUp(0) && m_freeGravityWells.Count > 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            float distance;
            plane.Raycast(ray, out distance);
            Vector3 spawnPosition = ray.GetPoint(distance);
            spawnPosition.z = 0f;

            SpawnGravityWell(spawnPosition);
        }

        if (portal.activeSelf != true && player.energy >= successEnergy) // Open portal
        {
            portal.SetActive(true);
        }

        if(!gameOver && ((player.energy + energyLeft) < successEnergy || player.energy <= 0.0f))
        {
            // Game over
            Debug.LogFormat("Game Over - EnergyLeft: {0}, SuccessEnergy: {1}, PlayerEnergy: {2}", energyLeft, successEnergy, player.energy);
            player.PlayDefeatSound();
            StartCoroutine(End(2.0f));
        }

        energyLevel.sizeDelta = new Vector2((player.energy / maxEnergy) * 600f, 10f);
        energyLeftInLevel.sizeDelta = new Vector2(((player.energy + energyLeft) / maxEnergy) * 600f, 10f);
    }

    public void SpawnGravityWell(Vector3 position)
    {
        if (m_freeGravityWells.Count > 0)
        {
            GravityWell well = Instantiate<GravityWell>(gravityWellPrefab, position, Quaternion.identity, gravityWellParent);
            int shaderUniform = m_freeGravityWells.Dequeue();
            well.Register(this, shaderUniform);
        }
    }

    public void EnterPortal()
    {
        // Win
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Unlock the next level
        PlayerPrefs.SetInt(string.Format("Level.Unlocked.{0}", sceneIndex + 1), 1);

        // Update best time if it's smaller than previous run
        float lastBestTime = PlayerPrefs.GetFloat(string.Format("Level.BestTime.{0}", sceneIndex), 0f);
        PlayerPrefs.SetFloat(string.Format("Level.BestTime.{0}", sceneIndex), lastBestTime);

        levelDone = true;
        player.PlayVictorySound();
        StartCoroutine(End(1.0f));
    }

    private void PauseGame()
    {
        m_pauseMenuVisible = true;
        m_animatingMenu = true;
        inputEnabled = false;
        pauseMenu.gameObject.SetActive(true);
        pauseMenu.alpha = 0f;

        Tween.Callback(1f, 0f, 0.4f, (float t) => { Time.timeScale = t; });

        Tween.Track().Alpha(pauseMenu, 1f, 0.4f).Action(() =>
        {
            m_animatingMenu = false;
        });
    }

    private void ContinueGame()
    {
        Tween.Track().Alpha(pauseMenu, 0f, 0.4f).Action(() =>
        {
            m_animatingMenu = false;
            m_pauseMenuVisible = false;
            pauseMenu.gameObject.SetActive(false);

            inputEnabled = true;
        });

        Tween.Track().Delay(0.2f).Callback(0f, 1f, 0.4f, (float t) => { Time.timeScale = t; });
    }

    private void ShowGameOverMenu()
    {
        m_gameOverMenuVisible = true;
        m_animatingMenu = true;
        inputEnabled = false;
        gameOverMenu.gameObject.SetActive(true);
        gameOverMenu.alpha = 0f;

        Tween.Callback(1f, 0f, 0.4f, (float t) => { Time.timeScale = t; });

        Tween.Track().Alpha(gameOverMenu, 1f, 0.4f).Action(() =>
        {
            m_animatingMenu = false;
        });
    }

    private void ShowGameDoneMenu()
    {
        m_animatingMenu = true;
        inputEnabled = false;
        gameDoneMenu.gameObject.SetActive(true);
        gameDoneMenu.alpha = 0f;

        Tween.Callback(1f, 0f, 0.4f, (float t) => { Time.timeScale = t; });

        Tween.Track().Alpha(gameDoneMenu, 1f, 0.4f).Action(() =>
        {
            m_animatingMenu = false;
        });
    }

    private IEnumerator End(float endTime)
    {
        gameOver = true;
        float time = 0.0f;
        while (time < endTime)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        if (levelDone)
            ShowGameDoneMenu();
        else
            ShowGameOverMenu();
    }
}
