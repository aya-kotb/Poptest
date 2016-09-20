using UnityEngine;

public class LoadingBox:MonoBehaviour
{

	public Vector3 startPos;
	public Vector3 target;

	public float passTime;
	public int numBeforeBreak;
	public float delayTime;
	private int count = 0;
	private float delay;


	// Use this for initialization
	void Start()
	{
		startPos.x = gameObject.transform.position.x;
		target.x = startPos.x;
		Repeat();

	}

	// Update is called once per frame
	void Update()
	{

	}

	//void StartBoxMove() {
	//	LeanTween.move(this.gameObject, target, passTime).setOnComplete(Repeat);
	//}

	void Repeat()
	{
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
		if(count >= numBeforeBreak)
		{
			delay = delayTime;
			count = 0;
		}
		else
		{
			delay = 0f;
		}

		this.gameObject.transform.position = startPos;
		LeanTween.move(this.gameObject, target, passTime).setOnComplete(Repeat).setDelay(delay);
		LeanTween.rotate(this.gameObject, new Vector3(0f, 0f, -90f), passTime).setDelay(delay);

		count++;
	}
}
