using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(0,1)]
    public float easing;
    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
        if(target != null)
        {
            if(easing <= 0)
                transform.position = target.position + offset;
            else
            {
                Vector3 targetLocation = target.position + offset;
                Vector3 dif = targetLocation - transform.position;
                transform.position += (dif / easing) * Time.deltaTime;
            }
        }
    }
}
