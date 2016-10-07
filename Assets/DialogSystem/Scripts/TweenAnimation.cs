using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TweenAnimation : MonoBehaviour
{
    public UnityEvent onComplete = new UnityEvent();
    bool end;
    float time;
    public float duration = 1;

    public bool loop = false;

    public AnimationData position;
    public AnimationData rotation;
    public AnimationData scale;
    RectTransform rect;
    // Use this for initialization
    void Start ()
    {
        if (position != null)
            position.duration = duration;
        if (rotation != null)
            rotation.duration = duration;
        if (scale != null)
            scale.duration = duration;
        rect = GetComponent<RectTransform>();
    }

    public void SetAnimatioinData(AnimationData posTemplate = null, AnimationData rotTemplate = null, AnimationData sclTemplate = null)
    {
        if(posTemplate != null)
        {
            position = new AnimationData(posTemplate.animation, posTemplate.start, posTemplate.end, duration);
        }

        if (rotTemplate != null)
        {
            rotation = new AnimationData(rotTemplate.animation, rotTemplate.start, rotTemplate.end, duration);
        }

        if (sclTemplate != null)
        {
            scale = new AnimationData(sclTemplate.animation, sclTemplate.start, sclTemplate.end, duration);
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        time += Time.deltaTime;
        if (time >= duration)
        {
            time = duration;
            end = true;
        }
        
        if (position != null)
        {
            if(rect != null)// if it is a ui element
                rect.anchoredPosition = position.GetValAtTime(time);
            else
                transform.position = position.GetValAtTime(time);
        }
            
        if (rotation != null)
            transform.eulerAngles = rotation.GetValAtTime(time);
        if (scale != null)
            transform.localScale = scale.GetValAtTime(time);
        
        if (end)
        {
            onComplete.Invoke();
            if(loop)
            {
                time = 0;
                end = false;
            }
            else
                GameObject.Destroy(this);
        }
    }
}
