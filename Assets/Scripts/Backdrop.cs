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
				float xx = (float)x * size;
				float yy = (float)y * size;
				Instantiate(prefab, new Vector3(xx, yy, transform.position.z), Quaternion.identity, transform);
			}
	}
}
