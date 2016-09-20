using UnityEngine;
using System.Collections;

namespace Poptropica2
{
    public class StateMachine : MonoBehaviour
    {
        public State initialState;
        private State _currentState;

        public State currentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                ChangeToState(value);
            }
        }


        public bool stateMachineIsActive = true;

        public enum StateUpdateMode
        {
            EveryFrame,
            EveryFixedUpdate,
            EverySecond,
            Manually

        }

        public StateUpdateMode updateMode;
        private State stateLastFrame;
        // mostly used to prevent infinite loops when insta-following multiple states.

        void Start()
        {
            currentState = initialState;
            if (initialState == null)
            {
                DebugLogFormat("Initial state is null!");
            }
            else
            {
                DebugLogFormat("Initial state set to {0}", initialState.stateName);
            }
        }

        private float lastUpdated = -1f;

        void Update()
        {
            if (updateMode == StateUpdateMode.EveryFrame)
            {
                UpdateStateAndEvaluate();
            }
            else if (updateMode == StateUpdateMode.EverySecond)
            {
                if (Time.time > lastUpdated + 1f)
                {
                    UpdateStateAndEvaluate();
                }
            }
        }

        void FixedUpdate()
        {
            if (updateMode == StateUpdateMode.EveryFixedUpdate)
            {
                UpdateStateAndEvaluate();
            }
        }

        public void UpdateStateAndEvaluate()
        {
            if (!stateMachineIsActive)
                return;
            lastUpdated = Time.time;

            if (_currentState != null)
            {
                _currentState.OnStateUpdate();

                for (int l = 0; l < _currentState.links.Length; l++)
                {
                    StateLink thisLink = _currentState.links[l];
                    bool conditionsAreTrue = true;
                    if (thisLink.conditions != null)
                    {
                        for (int c = 0; c < thisLink.conditions.Length; c++)
                        {
                            if (!thisLink.conditions[c].Evaluate())
                            {
                                conditionsAreTrue = false;
                                break;
                            }
                        }
                    }
                    if (conditionsAreTrue)
                    {
                        FollowStateLink(thisLink);
                    }
                }
            }
            else
            {
                DebugLogFormat("Current state is null!");
            }
        }

        public void FollowStateLink(StateLink link)
        {
            if (link.linkedState == null)
            {
                DebugLogFormat("Trying to follow link {0} but its linked state is null", link.linkLabel);
                stateMachineIsActive = false;
                currentState = null;
                return;
            }
            DebugLogFormat("Following state link {0} to state {1}", link.linkLabel, link.linkedState.stateName);
            currentState = link.linkedState;
        }

        public void ChangeToState(State newState)
        {
            if (_currentState != null)
                _currentState.OnExitState();
            if (newState != null)
            {
                _currentState = newState;
                _currentState.OnEnterState();
            }

        }

        public bool verbose = true;

        public void DebugLogFormat(string debug, params object[] args)
        {
            DebugLogFormat(debug, null, args);
        }

        public void DebugLogFormat(string debug, Object context, params object[] args)
        {
            if (context == null)
                context = this;
            string prefix = string.Format("{0} ({1:#.00}):  ", gameObject.name, Time.time);
            string debugString = string.Format(debug, args);
            lastDebug = prefix + debugString;
            if (verbose)
            {
                Debug.Log(prefix + debugString, context);
            }

        }

        public string lastDebug = "None";
    }
}