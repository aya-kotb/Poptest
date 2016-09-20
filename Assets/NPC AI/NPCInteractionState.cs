using UnityEngine;
using System.Collections;
using Poptropica2;

namespace PopTropica2.NPCAI
{
	
	public class NPCInteractionState : State
	{
		/// <summary>
		/// Type of interction npc will have
		/// </summary>
		public enum InteractionType
		{
			None,
			Proximity,
			LocationOfPC,
			MouseClick,
			Manual
		}

		[Header ("If Detection is Proximity Based")]
		//target character to detect if it is in specified range
	public Transform targetCharacter;
		//range value
		public float minDistance;
		[Space]
		[Header ("If Detection is Location Based")]
		public BoxCastInfo boxCastInfo;

		/// <summary>
		/// layer mask for detecting click
		/// </summary>
		private LayerMask npcMask = (1 << 15);
		public InteractionType interactionType;

		void Awake ()
		{
			onEnterAction.AddListener (OnActivate);
			onExitAction.AddListener (OnDeactivate);

			switch (interactionType) {
			case InteractionType.None:
				break;
			case InteractionType.Proximity:
				onUpdateAction.AddListener (OnProximitySensor);
				break;
			case InteractionType.LocationOfPC:
				onUpdateAction.AddListener (OnLocationSensor);
				break;
			case InteractionType.MouseClick:
				onUpdateAction.AddListener (OnMouseClick);
				break;
			case InteractionType.Manual:
				break;
			}
		}

		/// <summary>
		/// Called on entering the state
		/// Initializes condition for state transition
		/// </summary>
		void OnActivate ()
		{
			links = new StateLink[1];
			links [0] = new StateLink ();
			links [0].conditions = new IStateTransitionCondition[1];
			links [0].conditions [0] = new NPCConstantCondition ();
			links [0].linkedState = GetComponent<NPCActionState> ();
		}

		void OnDeactivate ()
		{
			links = null;
		}

		/// <summary>
		/// Detect if target character is Within the specified range of npc character
		/// </summary>
		void OnProximitySensor ()
		{
			//based on distance condition will be set to true
			if (targetCharacter != null) {
				if (Vector3.Distance (targetCharacter.position, transform.position) < minDistance) {
					SetStateFinished ();
				}
			}
		}

		/// <summary>
		/// Detects if target character is within the specified box areaa
		/// </summary>
		void OnLocationSensor ()
		{
			if (Physics2D.BoxCast (boxCastInfo.origin, boxCastInfo.size, boxCastInfo.angle, boxCastInfo.direction, boxCastInfo.distance, boxCastInfo.layerMask, boxCastInfo.minDepth, boxCastInfo.maxDepth)) {
				SetStateFinished ();
			}
		}

		/// <summary>
		/// Detects click event on npc character
		/// </summary>
		void OnMouseClick ()
		{
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity, npcMask);
				if (hit.collider != null && hit.transform.GetComponent<NPCInteractionState> () == this) {
					SetStateFinished ();
				}
			}
		}

		/// <summary>
		/// Method that can be called manually to initiate state transition
		/// </summary>
		public void Execute ()
		{
			if (interactionType == InteractionType.Manual)
				SetStateFinished ();
		}

		/// <summary>
		/// Sets the condition true for state transition
		/// </summary>
		void SetStateFinished ()
		{
			((NPCConstantCondition)links [0].conditions [0]).isStateFinished = true;
		}

		/// <summary>
		/// Gizmos for level designers convenience
		/// </summary>
		void OnDrawGizmos ()
		{
			if (interactionType == InteractionType.Proximity) {
				Gizmos.DrawWireSphere (transform.position, minDistance);
			} else if (interactionType == InteractionType.LocationOfPC) {
				Gizmos.color = Color.green;
				Gizmos.matrix = Matrix4x4.TRS (boxCastInfo.origin, this.transform.rotation, Vector3.one);
				Gizmos.DrawWireCube (Vector2.zero, boxCastInfo.size);
				Gizmos.matrix = Matrix4x4.TRS (boxCastInfo.origin + (boxCastInfo.direction.normalized * boxCastInfo.distance), this.transform.rotation, Vector3.one);
				Gizmos.DrawWireCube (Vector2.zero, boxCastInfo.size);
				Gizmos.color = Color.cyan;
				Gizmos.matrix = Matrix4x4.TRS (boxCastInfo.origin, Quaternion.identity, Vector3.one);
				Gizmos.DrawLine (Vector2.zero, boxCastInfo.direction.normalized * boxCastInfo.distance);
			}
		}

		/// <summary>
		/// Contains Info for Physics2D.Boxcast
		/// </summary>
		[System.Serializable]
		public class BoxCastInfo
		{
			public Vector2 origin;
			public Vector2 size;
			public float angle;
			public Vector2 direction;
			public float distance = Mathf.Infinity;
			public LayerMask layerMask = Physics2D.DefaultRaycastLayers;
			public float minDepth = -Mathf.Infinity;
			public float maxDepth = Mathf.Infinity;
		}
	}
}