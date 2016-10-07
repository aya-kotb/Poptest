using UnityEngine;
using System.Collections;

public class SecurityConsoleCutters : MonoBehaviour {

	public float speed = 5f;

	public GameObject redWire;
	public GameObject greenWire;
	public GameObject yellowWire;
	public GameObject redWireCut;
	public GameObject greenWireCut;
	public GameObject yellowWireCut;
	public GameObject buttonGlow;

	private Animator animator;
	private bool redCut = false;
	private bool greenCut = false;
	private bool yellowCut = false;

	void Awake()
    {
		animator = GetComponent<Animator> ();
	}

	void Start()
    {
		//initialize wires
		redWireCut.SetActive(false);
		greenWireCut.SetActive(false);
		yellowWireCut.SetActive(false);
	}

	void Update ()
    {
        // get world position of mouse cursor
        Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        worldPos.z = 20f;

        // convert to local position based on popup parent
        Vector3 localPos = (worldPos - this.gameObject.transform.parent.transform.position);
        // calculate distance between current and dest position
        float dist = (transform.localPosition.x - localPos.x);
        // move gradually toward destination vector
        transform.localPosition = Vector3.Lerp (transform.localPosition, localPos, speed * Time.unscaledDeltaTime);
        // animation rotation
		transform.localRotation = Quaternion.Euler (0, 0, dist * 2);
	}

	void OnMouseDown()
    {
        // get world position of mouse cursor
        Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        // convert to local position based on popup parent
        Vector3 localPos = (worldPos - this.gameObject.transform.parent.transform.position);

		//if within wire range
        if (localPos.x > -5f && localPos.x < 2.5f)
        {
			//red wire cut
            if (localPos.y > 1.9f && localPos.y < 3.2f)
            {
				redWire.SetActive(false);
				redWireCut.SetActive(true);
				redCut = true;
				CheckFinshed ();
			}
			//green wire cut
            if (localPos.y > -0.9f && localPos.y < 0.9f)
            {
				greenWire.SetActive(false);
				greenWireCut.SetActive(true);
				greenCut = true;
				CheckFinshed ();
			}
			//yellow wire cut
            if (localPos.y > -3.3f && localPos.y < -1.7f)
            {
				yellowWire.SetActive(false);
				yellowWireCut.SetActive(true);
				yellowCut = true;
				CheckFinshed ();
			}
		}
	}

	void CheckFinshed ()
    {
        // animate cutters
        animator.SetBool("cut", true);
        // if all wires cut then turn off glow
		if (redCut && greenCut && yellowCut) {
			buttonGlow.SetActive (false);
		}
	}
}
