using UnityEngine;
using System.Collections;

/// <summary>
/// Computer screen button to animate keyboard out.
/// </summary>
public class ComputerScreenButtonOut : MonoBehaviour
{
	void OnMouseUp()
    {
        transform.parent.parent.gameObject.GetComponent<ComputerPopup>().AnimateKeyboardOut();
	}
}