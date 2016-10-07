using UnityEngine;
using System.Collections;

/// <summary>
/// Computer screen button to animate keyboard in.
/// </summary>
public class ComputerScreenButton : MonoBehaviour
{
	void OnMouseUp()
    {
        transform.parent.gameObject.GetComponent<ComputerPopup>().AnimateKeyboardIn();
	}
}