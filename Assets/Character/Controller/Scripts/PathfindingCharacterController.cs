using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{

	public class PathfindingCharacterController : MonoBehaviour, ICharacterController
	{
		public bool traversingLink = false;
		public float jumpDuration = 0.5f;
		public float jumpStartTime;
		private OffMeshLinkData _currLink;
		private StandardCharacterModel standardCharacterModel;

		void Start ()
		{
			state = new InputControlGroup ();
			agent.autoTraverseOffMeshLink = false;
			agent.updateRotation = false;
			standardCharacterModel = agent.GetComponent<StandardCharacterModel> ();
			jumpDuration = standardCharacterModel.jumpDuration;
		}

		public void Update ()
		{
			Vector3 charPos = followingCharacter.transform.position;
			if ((agent.transform.position - transform.position).sqrMagnitude > 0.1f)
				agent.transform.position = new Vector3 (charPos.x, charPos.y, navMeshZ);
			//if agent reaches to any offmeshlink, we will manually implement jump or whatever needed
			if (agent.isOnOffMeshLink) {
				//Set once
				if (!traversingLink) {
					Debug.Log ("Off Mesh Link: " + agent.currentOffMeshLinkData.linkType.ToString ());
					traversingLink = true;
					standardCharacterModel.jumpStartTime = jumpStartTime = Time.time;
					_currLink = agent.currentOffMeshLinkData;
					state.Insert (InputControl.Jump, IsJumping ());
				} else {
					var currentJumpTime = Time.time - jumpStartTime;
					if (currentJumpTime < jumpDuration) {
						//lerp from link start to link end in time to animation
						var tlerp = currentJumpTime / jumpDuration;
						//straight line from startlink to endlink
						var newPos = Vector3.Lerp (_currLink.startPos, _currLink.endPos, tlerp);
						//add the 'hop'
						newPos.y += 2f * Mathf.Sin (Mathf.PI * tlerp);
						agent.transform.position = newPos;
					} else {
						//Called when agent reached to end point of current offmeshlink
						state.Insert (InputControl.Jump, false);
						traversingLink = false;
						agent.transform.position = agent.currentOffMeshLinkData.endPos;
						agent.CompleteOffMeshLink ();
					}
				}
			} else {
				if (Vector3.Distance (agent.steeringTarget, agent.transform.position) < 0.1f) {
					agent.transform.position = agent.steeringTarget;
				}
				//Debug.Log("Next off mesh link: " + (agent.nextOffMeshLinkData.valid ? agent.nextOffMeshLinkData.linkType.ToString() : "INVALID") );
			}
		}

		public void SetLookPosition (Vector2 position)
		{
			lookPosition = position;
		}

		public void SetMovePosition (Vector2 position)
		{
			Vector3 pos3d = new Vector3 (position.x, position.y, navMeshZ);

			//Note to anyone reading this: NavMesh.RayCast DOES NOT RAYCAST. 
			//It doesn't do anything like you think it does.
			RaycastHit2D hit = Physics2D.Raycast (position, Vector2.down);
			if (hit.collider != null) {
				Debug.Log ("hit from " + pos3d);
				Debug.DrawLine (pos3d, hit.point, Color.green, 5f);
				agent.SetDestination (new Vector3 (hit.point.x, hit.point.y, navMeshZ));
			} else {
				Debug.Log ("No Hit");
				Debug.DrawLine (pos3d, pos3d + Vector3.down * 20f, Color.red, 1f);
			}
			SetLookPosition (position);
		}

		public void HaltMovement ()
		{
			if (agent.isOnNavMesh) {
				agent.transform.position = followingCharacter.transform.position;
				agent.SetDestination (followingCharacter.transform.position);
			} else {
				RaycastHit2D hit = Physics2D.Raycast (followingCharacter.transform.position, Vector2.down);
				if (hit.collider != null) {
					agent.transform.position = hit.point;
					agent.SetDestination (hit.point);

				} else {
					Debug.LogWarning ("character is not grounded, and could not be grounded.");
				}
			}
		}

		bool IsJumping ()
		{
			return _currLink.endPos.y > _currLink.startPos.y;
		}

		InputControlGroup state;
		public float lookDistanceThreshold = 4f;

		public static float navMeshZ = -1.5f;
		private Vector2 lookPosition = Vector2.zero;
		public StandardCharacterModel followingCharacter;
		public NavMeshAgent agent;

		public float HorizontalAxisDegree {
			get {
				float movement = agent.desiredVelocity.x;
				return Mathf.Clamp (movement, -1f, 1f);
			}
		}


		public float VerticalAxisDegree {
			get {
				return 0f;
			}
		}

		public InputControlGroup Inputs {
			get {
				return state;
			}
		}

		public float EyeLookAngle {
			get {
				float radianAngle = Mathf.Atan2 (lookPosition.y - followingCharacter.transform.position.y, lookPosition.x - followingCharacter.transform.position.x);
				return radianAngle * Mathf.Rad2Deg;
			}
		}

		public float EyeOffsetAmount {
			get {
				float rawAmount = (lookPosition - (Vector2)followingCharacter.transform.position).magnitude / lookDistanceThreshold;
				return Mathf.Clamp01 (rawAmount);
			}
		}
	}

}