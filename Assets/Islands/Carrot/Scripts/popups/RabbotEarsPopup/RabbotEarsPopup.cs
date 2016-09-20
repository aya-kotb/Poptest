using UnityEngine;
using System.Collections;
using Poptropica2.PopupSystem;

public class RabbotEarsPopup : PopupNonUI
{
	[HideInInspector]public GameObject ears;

	void Start ()
    {
		Vector3 earsPos = new Vector3 (0f, -1f, 0f);
		ears.transform.localPosition = earsPos;
	}
}