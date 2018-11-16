using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player Player = null;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clicked!");
        }
    }

}
