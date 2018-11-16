using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera = null;
    public Player player = null;
    public GravityWell gravityWellPrefab = null;
    public Transform gravityWellParent = null;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clicked!");

            Vector3 spawnPosition = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
            spawnPosition.z = 0f;

            GravityWell well = Instantiate<GravityWell>(gravityWellPrefab, spawnPosition, Quaternion.identity, gravityWellParent);
            well.AffectPlayer(player);
        }
    }

}
