using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skillster.Animation;

public class Tutorial : MonoBehaviour
{
    public GameManager gameManager;
    public Transform player;
    public Transform pickup;
    public Transform enemy;

    public CanvasGroup root;
    public Button button;


    public void OnLetsGo()
    {
        root.alpha = 0f;
        Tween.Track().Alpha(root, 0f, 0.5f).Action(() =>
        {
            gameManager.inputEnabled = true;
            gameObject.SetActive(false);
        });

        Tween.Callback(0f, 1f, 0.5f, (float t) => { Time.timeScale = t; });
    }

    private void Start()
    {
        gameManager.inputEnabled = false;
        button.interactable = false;

        Tween.Track().Delay(0.1f).Callback(1f, 0f, 1.8f, (float t) => { Time.timeScale = t; });
        Tween.Track().Delay(0.1f).Alpha(root, 1f, 1f);
        Tween.Track().Delay(4f).Action(() => { button.interactable = true; });
    }
}
