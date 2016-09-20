using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Poptropica2.Characters;

public class PathfindingGenerator : Editor {
	[MenuItem("Pathfinding/Generate Platforms")]
	public static void GeneratePathfindingPlatforms()
	{
		GameObject platformsGO = GameObject.Find("Pathfinding Platform");
		if (platformsGO != null) DestroyImmediate(platformsGO);

		platformsGO = new GameObject("Pathfinding Platform");

		MeshFilter platformMF = platformsGO.AddComponent<MeshFilter>();
		MeshRenderer meshRend = platformsGO.AddComponent<MeshRenderer>();
		platformsGO.AddComponent<MeshCollider>();
		platformsGO.isStatic = true;

		Mesh newMesh = new Mesh();

		EdgeCollider2D[] allEdges = FindObjectsOfType<EdgeCollider2D>();
		BoxCollider2D[] allBoxes = FindObjectsOfType<BoxCollider2D>();

		List<Quad> quads = new List<Quad>();

		float navMeshDepth = 4f;
		Vector3 depthOffset = new Vector3(0f, 0f, PathfindingCharacterController.navMeshZ + navMeshDepth * 0.5f);
		Vector3 negDepthOffset = new Vector3(0f, 0f, PathfindingCharacterController.navMeshZ - navMeshDepth * 0.5f);

		foreach (EdgeCollider2D edge in allEdges)
		{
			Vector3 lastWorldPoint = Vector3.zero;
			for (int p=0;p< edge.points.Length;p++)
			{
				Vector3 worldPoint = edge.transform.TransformPoint(edge.points[p]);
				if (lastWorldPoint != Vector3.zero)
				{
					quads.Add(new Quad(lastWorldPoint + negDepthOffset, worldPoint + negDepthOffset, worldPoint + depthOffset, lastWorldPoint + depthOffset));
				}
				lastWorldPoint = worldPoint;
			}
		}

		foreach (BoxCollider2D box in allBoxes)
		{
			if (box.isTrigger) continue;
			float xMin = box.offset.x - box.size.x;
			float xMax = box.offset.x + box.size.x;
			float yTop = box.offset.y + box.size.y;

			Vector3 left = box.transform.TransformPoint(xMin, yTop, 0f);
			Vector3 right = box.transform.TransformPoint(xMax, yTop, 0f);
			quads.Add(new Quad(left + negDepthOffset, right + negDepthOffset, right + depthOffset, left + depthOffset));
		}

		Vector3[] vertices = new Vector3[quads.Count * 4];
		int[] tris = new int[quads.Count * 6];

		for (int q=0;q<quads.Count;q++)
		{
//			Debug.Log("Adding Quad " + q);
			quads[q].Insert(vertices, q * 4, tris, q * 6);
		}

		newMesh.vertices = vertices;
		newMesh.triangles = tris;

		platformMF.mesh = newMesh;

		NavMeshBuilder.ClearAllNavMeshes();
		NavMeshBuilder.BuildNavMeshAsync();
		meshRend.enabled = false;
	}
	public class Quad
	{
		private Vector3[] points;

		public Quad(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
		{
			points = new Vector3[] { point1, point2, point3, point4 };
		}

		public void Insert(Vector3[] vertArray, int vertIndex, int[] triArray, int triIndex)
		{
			for (int p = 0; p < 4; p++)
			{
				vertArray[vertIndex + p] = points[p];
			}

			triArray[triIndex + 0] = vertIndex + 0;
			triArray[triIndex + 1] = vertIndex + 3;
			triArray[triIndex + 2] = vertIndex + 1;
			triArray[triIndex + 3] = vertIndex + 3;
			triArray[triIndex + 4] = vertIndex + 2;
			triArray[triIndex + 5] = vertIndex + 1;
		}
	}

}
