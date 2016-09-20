using UnityEngine;
using System.Collections;

public class LeafFall : MonoBehaviour 
{
    public AnimationCurve xCurve;
    public AnimationCurve xCurve2;
    public AnimationCurve yCurve;
    public AnimationCurve yCurve2;

    private float curveTimeX;
    private float curveTimeY;
    private Rigidbody2D rgb;
    private bool landed;

	// Use this for initialization
	void Start () 
    {
        curveTimeX = 0;
        curveTimeY = 0;
        landed = false;
        rgb = this.GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate() 
    {
        if (!landed)
        {
            curveTimeX += Time.deltaTime;
            if (curveTimeX >= xCurve.keys[xCurve.length - 1].time)
            {
                curveTimeX = 0;
            }

            curveTimeY += Time.deltaTime;
            if (curveTimeY >= yCurve.keys[yCurve.length - 1].time)
            {
                curveTimeY = 0;
            }
            
            Vector2 velocity = rgb.velocity;
            // find a random in between the two curves
            velocity.x = Random.Range(xCurve.Evaluate(curveTimeX), xCurve2.Evaluate(curveTimeX));
            velocity.y = Random.Range(yCurve.Evaluate(curveTimeY), yCurve2.Evaluate(curveTimeY));
            rgb.velocity = velocity;
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.name == "Leaf Catcher")
        {
            landed = true;
            rgb.velocity = Vector2.zero;
            rgb.freezeRotation = true;
        }            
    }
}
