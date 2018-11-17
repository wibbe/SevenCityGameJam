using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backdrop : MonoBehaviour
{
	public GameObject prefab = null;
	public int resolutionX = 10;
	public int resolutionY = 10;
	public float size = 5f;

	private void Awake()
	{
		for (int y = 0; y < resolutionY; y++)
			for (int x = 0; x < resolutionX; x++)
			{
				float xx = (float)(x - (resolutionX / 2)) * size;
				float yy = (float)(y - (resolutionY / 2)) * size;
				Instantiate(prefab, new Vector3(xx, yy, transform.position.z), Quaternion.identity, transform);
			}
	}

    private void OnDrawGizmosSelected()
    {
        int halfX = resolutionX / 2;
        int halfY = resolutionY / 2;
        float sizeX = size / transform.localScale.x;
        float sizeY = size / transform.localScale.y;
        Vector3 lowerLeft = Vector3.left * halfX * -sizeX + Vector3.down * halfY * -sizeY;
        Vector3 upperLeft = Vector3.left * halfX * -sizeX + Vector3.down * halfY * sizeY;
        Vector3 lowerRight= Vector3.left * halfX * sizeX + Vector3.down * halfY * -sizeY;
        Vector3 upperRight= Vector3.left * halfX * sizeX + Vector3.down * halfY * sizeY;
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(lowerLeft, upperLeft);
        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(upperRight, lowerRight);
        Gizmos.DrawLine(lowerRight, lowerLeft);
    }
}
