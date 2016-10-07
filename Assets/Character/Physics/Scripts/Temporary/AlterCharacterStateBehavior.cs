using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This class is used to change animations of different states of the Animator Controller during runtime
    /// It uses AnimatorOverrideController for doing so
    /// 
    /// There are some limitations to using AnimatorOverrideController -
    /// 
    /// 1. It can only access a string parameter
    /// 2. There's possibly a Unity Bug - which will make the AnimatorOverrideController's states only accessible with clip names
    ///     For example - If the Run State's animation clip has been changed from "Run" to "AlternateRun"; 
    ///     to override the state again the second time the name "AlternateRun" has to be used
    ///     instead of "Run" and so on.
    ///     
    ///     Because of that a ClipNameTracker class is being used
    /// 
    /// 
    /// This class will change animation clips based on the current model type. Alternate model type is an example.
    /// The alternate clips can be assigned to within the inspector
    /// Only those states which need overriding should be assigned. Other states can be left blank
    /// </summary>
    public class AlterCharacterStateBehavior : MonoBehaviour
    {
        public CharacterModel.CharacterModelTypes characterModelType;
        CharacterModel.CharacterModelTypes prevModelType;

        public StandardAnimationClips alternateClips;
        StandardAnimationClips defaultClips;

        public Animator animator;

        List<ClipNameTracker> clipNamesTracker;

        void Awake()
        {
            FillDefaultClips();

            clipNamesTracker = new List<ClipNameTracker>();
            for (int i = 0; i < StandardAnimatorStates.animatorStates.Count; i++)
            {
                ClipNameTracker clipName = new ClipNameTracker();
                clipName.changedName = clipName.defaultName = StandardAnimatorStates.animatorStates[i];
                clipNamesTracker.Add(clipName);
            }
        }

        void Start()
        {
            prevModelType = characterModelType;
            if (characterModelType == CharacterModel.CharacterModelTypes.Alternate)
            {
                ChangeAnimationClips(alternateClips);
            } 
        }

        // Use this for initialization
        void Update()
        {	
            CheckForStateChange();
        }

        /// <summary>
        /// This function for is runtime testing of animation clip change
        /// From Alternate to Standard and vice versa. Or if other states are added later, those can be accessed too
        /// </summary>
        void CheckForStateChange()
        {
            if (prevModelType != characterModelType)
            {
                if (characterModelType == CharacterModel.CharacterModelTypes.Alternate)
                {
                    ChangeAnimationClips(alternateClips);
                }
                else
                {
                    ChangeAnimationClips(defaultClips);
                }

                prevModelType = characterModelType;
            }
        }

        /// <summary>
        /// Changes the animation clips by calling the AnimatorOverride function
        /// Changes only those whose overridable animation clips are assigned in the inspector
        /// </summary>
        /// <param name="clips">Clips.</param>
        void ChangeAnimationClips(StandardAnimationClips clips)
        {
            for (int i = 0; i < clipNamesTracker.Count; i++)
            {
                string clipDefaultName = clipNamesTracker[i].defaultName;
                string clipChangedName = clipNamesTracker[i].changedName;
                AnimatorOverride(animator, clips.GetAnimationClipWithName(clipDefaultName).clip, ref clipChangedName); 
                clipNamesTracker[i].changedName = clipChangedName;
            }
			     
        }

        /// <summary>
        /// Overrides the animations in the current AnimatorController
        /// Uses AnimatorOverrideController to do so
        /// 
        /// Because of a problem with Unity it requires a reference type string "clipName"
        /// This variable stores the latest clip name of the AnimatorController for the current state
        /// This name is required for that state's animation to be changed again
        /// 
        /// AnimatorOverriding can only be done using strings
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="clip">Clip.</param>
        /// <param name="clipName">Clip name.</param>
        void AnimatorOverride(Animator animator, AnimationClip clip, ref string clipName)
        {
            if (clip == null)
            {
                return;
            }

            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController();
            animatorOverride.runtimeAnimatorController = runtimeAnimatorController;
            animatorOverride[clipName] = clip; 
            animator.runtimeAnimatorController = animatorOverride;
            clipName = clip.name;
        }

        /// <summary>
        /// Fills the default clips.
        /// If later the model type is set back to standard, the default clips can be loaded back instead of the overriden clips
        /// </summary>
        void FillDefaultClips()
        {
            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
            AnimatorOverrideController animatorOverride = new AnimatorOverrideController();
            animatorOverride.runtimeAnimatorController = runtimeAnimatorController;

            defaultClips = new StandardAnimationClips();

            for (int i = 0; i < StandardAnimatorStates.animatorStates.Count; i++)
            {
                defaultClips.GetAnimationClipWithName(StandardAnimatorStates.animatorStates[i]).clip = animatorOverride[StandardAnimatorStates.animatorStates[i]];
            }

        }

        /// <summary>
        /// This class is for the Animation Clip Names to be used with the AnimatorController
        /// The defaultName variable is the same throughout once it's assigned
        /// 
        /// The changedName variable is needed because once a state in the AnimatorController is overriden 
        /// - it can only be overriden once again with this changedName - It might be a Unity bug
        /// </summary>
        class ClipNameTracker
        {
            public string defaultName;
            public string changedName;
        }
    }

    /// <summary>
    /// Animation Clips for the Standard Animator Controller
    /// </summary>
    [System.Serializable]
    public class StandardAnimationClips
    {
        List<AnimationClipWithStateName> clipsWithStateNames;

        public AnimationClipWithStateName climbUp;
        public AnimationClipWithStateName climbDown;
        public AnimationClipWithStateName jump;
        public AnimationClipWithStateName fall;
        public AnimationClipWithStateName walk;
        public AnimationClipWithStateName run;
        public AnimationClipWithStateName push;
        public AnimationClipWithStateName jumpSpin;
        public AnimationClipWithStateName landSpin;
        public AnimationClipWithStateName stand;
        public AnimationClipWithStateName roll;
        public AnimationClipWithStateName swim;
        public AnimationClipWithStateName swimTread;
        public AnimationClipWithStateName swimJump;
        public AnimationClipWithStateName swimLand;
        public AnimationClipWithStateName skid;
        public AnimationClipWithStateName hurt;

        public StandardAnimationClips()
        {
            climbUp = new AnimationClipWithStateName(StandardAnimatorStates.climbUp);
            climbDown = new AnimationClipWithStateName(StandardAnimatorStates.climbDown);
            jump = new AnimationClipWithStateName(StandardAnimatorStates.jump);
            fall = new AnimationClipWithStateName(StandardAnimatorStates.fall);
            walk = new AnimationClipWithStateName(StandardAnimatorStates.walk);
            run = new AnimationClipWithStateName(StandardAnimatorStates.run);
            push = new AnimationClipWithStateName(StandardAnimatorStates.push);
            jumpSpin = new AnimationClipWithStateName(StandardAnimatorStates.jumpSpin);
            landSpin = new AnimationClipWithStateName(StandardAnimatorStates.landSpin);
            stand = new AnimationClipWithStateName(StandardAnimatorStates.stand);
            roll = new AnimationClipWithStateName(StandardAnimatorStates.roll);
            swim = new AnimationClipWithStateName(StandardAnimatorStates.swim);
            swimTread = new AnimationClipWithStateName(StandardAnimatorStates.swimTread);
            swimJump = new AnimationClipWithStateName(StandardAnimatorStates.swimJump);
            swimLand = new AnimationClipWithStateName(StandardAnimatorStates.swimLand);
            skid = new AnimationClipWithStateName(StandardAnimatorStates.skid);
            hurt = new AnimationClipWithStateName(StandardAnimatorStates.hurt);

            clipsWithStateNames = new List<AnimationClipWithStateName>();
            clipsWithStateNames.Add(climbUp);
            clipsWithStateNames.Add(climbDown);
            clipsWithStateNames.Add(jump);
            clipsWithStateNames.Add(fall);
            clipsWithStateNames.Add(walk);
            clipsWithStateNames.Add(run);
            clipsWithStateNames.Add(push);
            clipsWithStateNames.Add(jumpSpin);
            clipsWithStateNames.Add(landSpin);
            clipsWithStateNames.Add(stand);
            clipsWithStateNames.Add(roll);
            clipsWithStateNames.Add(swim);
            clipsWithStateNames.Add(swimTread);
            clipsWithStateNames.Add(swimJump);
            clipsWithStateNames.Add(swimLand);
            clipsWithStateNames.Add(skid);
            clipsWithStateNames.Add(hurt);
        }

        /// <summary>
        /// Gets an AnimationClipWithStateName object with state name
        /// This is using a string comparison as there's no other option for now
        /// </summary>
        /// <returns>The animation clip with name.</returns>
        /// <param name="stateName">State name.</param>
        public AnimationClipWithStateName GetAnimationClipWithName(string stateName)
        {
            for (int i = 0; i < clipsWithStateNames.Count; i++)
            {
                if (clipsWithStateNames[i].StateName.CompareTo(stateName) == 0)
                {
                    return clipsWithStateNames[i];
                }
            }
            return null;
        }
    }
        
    /// <summary>
    /// This class is used for storing animation clips and their state names
    /// </summary>
    [System.Serializable]
    public class AnimationClipWithStateName
    {
        public AnimationClip clip;
        string staneName;

        public string StateName
        {
            get
            {
                return staneName;
            }
        }


        public AnimationClipWithStateName(string name)
        {
            staneName = name;
        }
    }

    /// <summary>
    /// Standard animator state names stored in variables and a list for looping
    /// </summary>
    public class StandardAnimatorStates
    {
        public static List<string> animatorStates;

        public static string climbUp = "climbUp";
        public static string climbDown = "climbDown";
        public static string jump = "jump";
        public static string fall = "fall";
        public static string walk = "walk";
        public static string run = "run";
        public static string push = "push";
        public static string jumpSpin = "jumpSpin";
        public static string landSpin = "landSpin";
        public static string stand = "stand";
        public static string roll = "roll";
        public static string swim = "swim";
        public static string swimTread = "swimTread";
        public static string swimJump = "swimJump";
        public static string swimLand = "swimLand";
        public static string skid = "skid";
        public static string hurt = "hurt";

        static StandardAnimatorStates()
        {
            animatorStates = new List<string>();
            animatorStates.Add(climbUp);
            animatorStates.Add(climbDown);
            animatorStates.Add(jump);
            animatorStates.Add(fall);
            animatorStates.Add(walk);
            animatorStates.Add(run);
            animatorStates.Add(push);
            animatorStates.Add(jumpSpin);
            animatorStates.Add(landSpin);
            animatorStates.Add(stand);
            animatorStates.Add(roll);
            animatorStates.Add(swim);
            animatorStates.Add(swimTread);
            animatorStates.Add(swimJump);
            animatorStates.Add(swimLand);
            animatorStates.Add(skid);
            animatorStates.Add(hurt);
        }
    }
}
