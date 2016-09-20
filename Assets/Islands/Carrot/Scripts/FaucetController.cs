using UnityEngine;
using System.Collections;

public class FaucetController : MonoBehaviour {
	public ParticleSystem shower;

	void OnMouseDown() {
		LeanTween.rotateAround(gameObject, Vector3.forward, -1000f, 3f).setEase(LeanTweenType.easeInOutSine);

		if (!shower.isPlaying) {
			shower.Play ();
		}
	}
}
