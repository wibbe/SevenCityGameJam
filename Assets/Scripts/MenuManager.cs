using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;
    public Transform[] playerTargets;


    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        //StartCoroutine(GravityWellSpawner());
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
