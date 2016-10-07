using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard cling to hit.
    /// </summary>
    public class StandardClingToHit:CharacterPhysicsBehavior
    {
        float clingThreshhold = 0.1f;
        float clingSpeed = 3f;
        float lateralForce = 2000f;

        public float ClingThreshhold
        {
            get
            {
                return clingThreshhold;
            }
            set
            {
                clingThreshhold = value;
            }
        }

        public float ClingSpeed
        {
            get
            {
                return clingSpeed;
            }
            set
            {
                clingSpeed = value;
            }
        }

        public float LateralForce
        {
            get
            {
                return lateralForce;
            }
            set
            {
                lateralForce = value;
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            InitializeClingToHit();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            CheckForLateralForce(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {

        }

        /// <summary>
        /// Initializes the cling to hit functionality
        /// </summary>
        void InitializeClingToHit()
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            Vector2 stoppedVelocity = new Vector2(0f, rigidBody.velocity.y);
            if (ValidVelocity(stoppedVelocity))
            {
                rigidBody.velocity = stoppedVelocity;
            }
        }

        /// <summary>
        /// Checks for lateral force.
        /// </summary>
        /// <param name="animator">Animator.</param>
        void CheckForLateralForce(Animator animator)
        {
            CharacterModel model = parentStateBehavior.Model;
            Rigidbody2D rigidBody = parentStateBehavior.Rb;

            float speedInputX = animator.GetFloat(StandardAnimatorHashes.speedPctHoriz);
            float distanceFromHitPosX = Mathf.Abs(model.HitPosition.x - model.transform.position.x);
            float sign = (model.FacingRight ? 1f : -1f);

            if (speedInputX > 0.1f) 	//Allow lateral movement
            { 
                rigidBody.AddForce(Vector2.right * lateralForce * speedInputX * Time.fixedDeltaTime * sign);
            }
            else if (distanceFromHitPosX > clingThreshhold) 	//But if no lateral movement, cling to the center
            { 
                Vector3 targetPosition = new Vector3(model.HitPosition.x, model.transform.position.y, model.transform.position.z);
                model.transform.position = Vector3.MoveTowards(model.transform.position, targetPosition, Time.fixedDeltaTime * clingSpeed);
            }
        }		
    }
}
