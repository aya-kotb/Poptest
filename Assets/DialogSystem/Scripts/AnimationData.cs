using UnityEngine;
using System.Collections;
[System.Serializable]
public class AnimationData
{
    public AnimationCurve animation;
    public float duration;
    public Vector3 start;
    public Vector3 end;

    public AnimationData(AnimationCurve curve, Vector3 start, Vector3 end, float duration = 1)
    {
        animation = curve;
        this.duration = duration;
        this.start = start;
        this.end = end;
    }

    public Vector3 GetValAtTime(float time)
    {
        Vector3 difference = end - start;
        return start + difference * (animation.Evaluate(time / duration));
    }
}