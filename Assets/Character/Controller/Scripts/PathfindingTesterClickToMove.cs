using UnityEngine;
using System.Collections;
using Poptropica2.Characters;

public class PathfindingTesterClickToMove : MonoBehaviour {
	public Vector2 mousePosition = Vector3.zero;

	private PathfindingCharacterController controller;

	// Use this for initialization
	void Start () {
		controller = GetComponent<PathfindingCharacterController>();
		controller.HaltMovement();
	}
	
	// Update is called once per frame
	void Update () {
		mousePosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0f);
		if (Input.GetMouseButtonDown(0))
		{
			controller.SetMovePosition(mousePosition);
		}
	}
}
