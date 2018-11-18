using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartLevel : MonoBehaviour
{
    public int level = 0;
    public bool alwaysActive = false;

    public TextMeshProUGUI bestTimeText;

    public void OnClick()
    {
        SceneManager.LoadScene(level);
    }

    private void Start()
    {
        bool active = PlayerPrefs.GetInt(string.Format("Level.Unlocked.{0}", level), 0) > 0;
        GetComponent<Button>().interactable = active || alwaysActive;

        float bestTime = PlayerPrefs.GetFloat(string.Format("Level.BestTime.{0}", level), 0f);

        if (active || alwaysActive)
        {
            int minutes = Mathf.FloorToInt(bestTime / 60f);
            int seconds = Mathf.FloorToInt(bestTime - (minutes * 60f));

            bestTimeText.text = string.Format("Best Time\n{0}:{1:00}", minutes, seconds);
        }
        else
        {
            bestTimeText.text = "Locked";
        }
    }
}
