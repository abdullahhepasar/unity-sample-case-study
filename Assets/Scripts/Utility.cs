
using UnityEngine;

/*
 * 
 * 
 * Complete the functions below.  
 * For sure, they don't belong in the same class. This is just for the test so ignore that.
 * 
 * 
 */

using System.Collections.Generic;
using System.Linq;

public static class Utility
{


	public static GameObject[] GetObjectsWithName(string name)
	{

		/*
		 * 
		 *	Return all objects in the scene with the specified name. Don't think about performance, do it in as few lines as you can. 
		 * 
		 */

		List<GameObject> GetAllObjectsOnlyInScene = new List<GameObject>();
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			if (go.name == name)
				GetAllObjectsOnlyInScene.Add(go);
		}

		GameObject[] gameObjects = new GameObject[GetAllObjectsOnlyInScene.Count];
		int length = gameObjects.Length;

		for (int i = 0; i < length; i++)
        {
			gameObjects[i] = GetAllObjectsOnlyInScene[i];
		}

		if (gameObjects != null)
			return gameObjects;

		return null;
	}


	public static bool CheckCollision(Ray ray, float maxDistance, int layer)
	{
		/*
		 * 
		 *	Perform a raycast using the ray provided, only to objects of the specified 'layer' within 'maxDistance' and return if something is hit. 
		 * 
		 */

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, maxDistance))
			if (hit.transform.gameObject.layer == layer)
				return true;

		return false;
	}





	public static Vector2[] GeneratePoints(int size)
	{

		/*
		 * Generate 'size' number of random points, making sure they are distributed as evenly as possible (Trying to achieve maximum distance between every neighbor).
		 * Boundary corners are (0, 0) and (1, 1). (Point (1.2, 0.45) is not valid because it's outside the boundaries. )
		 * Is there a known algorithm that achieves this?
		 */


		// There is no algorithm that can achieve exactly this. But there is an algorithm that I know close to this.
		// It is randomly located and able to distribute in a certain area close to equidistant.
		// Name: Poisson-Disc Sampling algorithm
		//https://www.jasondavies.com/poisson-disc/

		//You can find an example below:

		List<Vector2> points = PoissonDiscSampling.GeneratePoints(1, new Vector2(1, 1), size);

		Vector2[] generatePoints = new Vector2[points.Count];
		int Length = generatePoints.Length;

		for (int i = 0; i < Length; i++)
        {
			generatePoints[i] = points[i];
		}

		if (generatePoints != null)
			return generatePoints;

		//For test:		
		//void OnDrawGizmos()
		//{
		//	Gizmos.DrawWireCube(regionSize / 2, regionSize);
		//	if (points != null)
		//	{
		//		foreach (Vector2 point in points)
		//		{
		//			Gizmos.DrawSphere(point, displayRadius);
		//		}
		//	}
		//}

		return null;
	}


	public static Texture2D GenerateTexture(int width, int height, Color color)
	{

		/*
		 * Create a Texture2D object of specified 'width' and 'height', fill it with 'color' and return it. Do it as performant as possible.
		 */

		Texture2D texture = new Texture2D(width, height);
		Color[] pixels = Enumerable.Repeat(color, width * height).ToArray();
		texture.SetPixels(pixels);
		texture.Apply();

		if (texture != null)
			return texture;

		return null;
	}




}

public static class PoissonDiscSampling
{

	public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
	{
		float cellSize = radius / Mathf.Sqrt(2);

		int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
		List<Vector2> points = new List<Vector2>();
		List<Vector2> spawnPoints = new List<Vector2>();

		spawnPoints.Add(sampleRegionSize / 2);
		while (spawnPoints.Count > 0)
		{
			int spawnIndex = Random.Range(0, spawnPoints.Count);
			Vector2 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < numSamplesBeforeRejection; i++)
			{
				float angle = Random.value * Mathf.PI * 2;
				Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
				Vector2 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);
				if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
				{
					points.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}

		return points;
	}

	static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
	{
		if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
		{
			int cellX = (int)(candidate.x / cellSize);
			int cellY = (int)(candidate.y / cellSize);
			int searchStartX = Mathf.Max(0, cellX - 2);
			int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
			int searchStartY = Mathf.Max(0, cellY - 2);
			int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

			for (int x = searchStartX; x <= searchEndX; x++)
			{
				for (int y = searchStartY; y <= searchEndY; y++)
				{
					int pointIndex = grid[x, y] - 1;
					if (pointIndex != -1)
					{
						float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
						if (sqrDst < radius * radius)
						{
							return false;
						}
					}
				}
			}
			return true;
		}
		return false;
	}
}