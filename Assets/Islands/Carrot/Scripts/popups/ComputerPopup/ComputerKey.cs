using UnityEngine;
using System.Collections;

/// <summary>
/// Computer key behavior
/// </summary>
public class ComputerKey : MonoBehaviour
{
    public Sprite button;
    public Sprite buttonDown;

    private string keyValue;
	private SpriteRenderer rend;
    private ComputerPopup popup;
    private Transform letter;
    private GameObject buttonSprite;

	void Start ()
    {
        buttonSprite = transform.Find("key_back").gameObject;
		rend = buttonSprite.GetComponent<SpriteRenderer> ();
        keyValue = this.name;
        popup = transform.parent.parent.gameObject.GetComponent<ComputerPopup>();
        letter = transform.Find("letter");
        // if no button sprite passed, then use default from popup
        if (button == null)
        {
            button = popup.keyButton;
            buttonDown = popup.keyButtonDown;
        }
	}

	void OnMouseDown()
    {
        rend.sprite = buttonDown;
		popup.TypeLetter (keyValue);
        if (letter != null)
        {
            letter.transform.Translate(new Vector3(0f, -0.1f, 0f));
        }
	}

	void OnMouseUp()
    {
        rend.sprite = button;
        if (letter != null)
        {
            letter.transform.Translate(new Vector3(0f, 0.1f, 0f));
        }
	}
}