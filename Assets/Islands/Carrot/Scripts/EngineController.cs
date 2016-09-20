using UnityEngine;
using System.Collections;

public class EngineController : MonoBehaviour {

	public GameObject switch1;
	public GameObject switch2;
	public GameObject switch3;
	public GameObject hook;
	public ParticleSystem particles;

	public GameObject[] rotators;

	private ParticleSystem.EmissionModule em;

	bool isRunning = false;

	// Use this for initialization
	void Start () {
		em = particles.emission;
		em.enabled = false;
		hook.GetComponent<Animation> ().Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		if (switch1.GetComponent<EngineSwitch> ().isOn == true
		    && switch2.GetComponent<EngineSwitch> ().isOn == true
		    && switch3.GetComponent<EngineSwitch> ().isOn == true) {
			if (!isRunning) {
				isRunning = true;
				foreach(GameObject rotator in rotators) {
					rotator.GetComponent<Rotator> ().isRotating = true;
				}
				hook.GetComponent<Animation> ().Play ();
				em.enabled = true;

			}
		} else {
			if (isRunning) {
				isRunning = false;
				foreach(GameObject rotator in rotators) {
					rotator.GetComponent<Rotator> ().isRotating = false;
				}
				//hook.GetComponent<Animation> ().enabled = false;
				hook.GetComponent<Animation> ().Stop ();
				em.enabled = false;
			}
		}
	}
}
