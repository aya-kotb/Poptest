using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Poptropica2.Characters;

/// <summary>
/// This class is a temporary event manager which is being used to change controllers during runtime
/// </summary>
public class EventManager
{
    public delegate void JumpState(bool isDoubleJumpingAllowed = false);
    public event JumpState OnJumpState;

    public delegate void OverrideController(ICharacterController overridingController);
    public event OverrideController OnOverrideController;

    private static EventManager instance = null;

    public static EventManager GetInstance()
    {
        if (instance == null)
        {
            instance = new EventManager();
        }
		
        return instance;
    }

    public void DispatchOnJumpState(bool isDoubleJumpingAllowed = false)
    {
        if (OnJumpState != null)
        {
            OnJumpState(isDoubleJumpingAllowed);
        }
    }

    public void DispatchOnOverrideController(ICharacterController overridingController)
    {
        if (OnOverrideController != null)
        {
            OnOverrideController(overridingController);
        }
    }
}
