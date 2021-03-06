﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skillster.Animation;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;
    public Transform[] playerTargets;
    public RectTransform menu;
    public RectTransform levelSelect;
    public AudioClip menuSound;

    private AudioSource audioSource;


    public void Play()
    {
        audioSource.Play();
        Tween.Pivot(menu, new Vector2(1.5f, 0.5f), 0.4f);
        Tween.Pivot(levelSelect, new Vector2(0.5f, 0.5f), 0.4f);
    }

    public void Quit()
    {
        audioSource.Play();
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnMenu()
    {
        audioSource.Play();
        SceneManager.LoadScene(0);
    }

    public void OnBack()
    {
        audioSource.Play();
        Tween.Pivot(menu, new Vector2(0.5f, 0.5f), 0.4f);
        Tween.Pivot(levelSelect, new Vector2(-0.5f, 0.5f), 0.4f);
    }

    public void OnCredits()
    {
        audioSource.Play();
        int creditScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(creditScene);
    }

    private void Start()
    {
        if (menu != null)
            menu.pivot = new Vector2(0.5f, 0.5f);

        if (levelSelect != null)
            levelSelect.pivot = new Vector2(-0.5f, 0.5f);

        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1f;
        StartCoroutine(ControlPlayer());
    }

    private IEnumerator GravityWellSpawner()
    {
        while (true)
        {
            float x = player.transform.position.x + Random.Range(-10f, 10f);
            float y = player.transform.position.y + Random.Range(-10f, 10f);

            gameManager.SpawnGravityWell(new Vector3(x, y, 0f));
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    private IEnumerator ControlPlayer()
    {
        WaitForSeconds sleep = new WaitForSeconds(0.05f);
        while (true)
        {
            int index = Random.Range(0, playerTargets.Length);
            player.target = playerTargets[index];

            while (true)
            {
                float distance = Vector3.Distance(playerTargets[index].position, player.transform.position);
                if (distance < 5f || Random.Range(0f, 1f) > 0.95f)
                    break;

                yield return sleep;
            }

            float x = player.transform.position.x + Random.Range(-5f, 5f);
            float y = player.transform.position.y + Random.Range(-5f, 5f);

            gameManager.SpawnGravityWell(new Vector3(x, y, 0f));
            //yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }
}
