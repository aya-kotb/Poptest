using UnityEngine;
using System.Collections;
using Poptropica2;
using Poptropica2.Characters;

namespace PopTropica2.NPCAI
{
	public class NPCActionState : State
	{
		/// <summary>
		/// Type of Action an NPC can perform
		/// </summary>
		public enum ActionType
		{
			None,
			FollowPlayer,
			AvoidPlayer,
			TriggerOnce,
		}

		public ActionType actionType;
		/// <summary>
		/// After action is performed whether npc will go back to interaction state
		/// </summary>
		public bool loop;

		/// <summary>
		/// Target character to follow or avoid
		/// </summary>
		public Transform targetCharacter;
		[Header ("If Detection is Location Based")]
		//Target to reach when target character enters specified area
	public Vector2 targetPosition;

		[Space]
		[Header ("If Detection is Proximity Based")]
		//mindistance at which npc will stop before PC while following
	public float minDistance;
		//navmesh agent for npc
		private NavMeshAgent agent;

		void Awake ()
		{
			agent = GetComponent<NavMeshAgent> ();
			onEnterAction.AddListener (OnActivate);
			onExitAction.AddListener (OnDeactivate);

			switch (actionType) {
			case ActionType.TriggerOnce:
				onUpdateAction.AddListener (SetStateFinished);
				break;
			case ActionType.FollowPlayer:
			case ActionType.AvoidPlayer:
				onUpdateAction.AddListener (UpdateAgent);
				break;
			case ActionType.None:
				break;
			}
		}

		/// <summary>
		/// To be executed when entering the state
		/// </summary>
		void OnActivate ()
		{
			links = new StateLink[1];
			links [0] = new StateLink ();
			links [0].conditions = new IStateTransitionCondition[1];
			links [0].conditions [0] = new NPCConstantCondition ();
			if (loop)
				links [0].linkedState = GetComponent<NPCInteractionState> ();

			if (actionType == ActionType.AvoidPlayer || actionType == ActionType.FollowPlayer)
				SetMovePosition ();
		}

		/// <summary>
		/// To do anything while leaving the state
		/// </summary>
		void OnDeactivate ()
		{
			links = null;
		}

		/// <summary>
		/// Checks if npc reached its desired location
		/// </summary>
		void UpdateAgent ()
		{
			if (!agent.pathPending) {
				if (agent.remainingDistance <= agent.stoppingDistance) {
					if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
						SetStateFinished ();
					}
				}
			}
		}

		/// <summary>
		/// Uses the pathfindingcharactercontroller to manuaaly move this character
		/// </summary>
		private void SetMovePosition ()
		{
			Vector2 dest = default(Vector2);
			if (actionType == ActionType.FollowPlayer) {
				agent.stoppingDistance = minDistance;
				dest = targetCharacter.position;
			} else if (actionType == ActionType.AvoidPlayer) {
				agent.stoppingDistance = 0;
				dest = targetPosition;
			}
			((PathfindingCharacterController)agent.GetComponent<StandardCharacterModel> ().ControllerContainer.Result).SetMovePosition (dest);
			
		}

		/// <summary>
		/// Sets the state transition
		/// </summary>
		void SetStateFinished ()
		{
			((NPCConstantCondition)links [0].conditions [0]).isStateFinished = true;
		}
	}
}