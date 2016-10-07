using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Smasher : MonoBehaviour
{
    public Transform top, bottom;
    public float topDefaultHeight, bottomDefaultHeight;

    public UnityEvent smashComplete;
    public UnityEvent releaseComplete;
    public UnityEvent startSmash;

    float time, duration, end, dif;

    public float smashDelay, smashTime, releaseDelay, releaseTime, smashHeight, releaseHeight;
    
    public void smash(float speed, float endHeight)//0 will be fully with drawn
    {
        time = 0;
        duration = speed;
        end =  endHeight;
        StartCoroutine("DoSmash");
    }

    IEnumerator DoSmash()
    {
        float startPosition = bottom.localPosition.y;
        float startScale = top.localScale.y;
        dif = end - startPosition;

        if (dif < 0)
            startSmash.Invoke();
        bool shouldSmash = dif != 0;
        float percent;
        while (shouldSmash)
        {
            time += Time.deltaTime;
            shouldSmash = (time < duration);
            if (!shouldSmash)
                time = duration;
            percent = time / duration;
            top.localScale = new Vector3(1, startScale + dif / topDefaultHeight * -percent, 1);
            bottom.localPosition = new Vector3(bottom.localPosition.x,startPosition + dif * percent, bottom.localPosition.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (dif < 0)
            smashComplete.Invoke();
        else
            releaseComplete.Invoke();
    }

    public void Smash()
    {
        StartCoroutine("DoDelaySmash");
    }

    IEnumerator DoDelaySmash()
    {
        yield return new WaitForSeconds(smashDelay);
        smash(smashTime, smashHeight);
    }

    public void Release()
    {
        StartCoroutine("DoDelayRelease");
    }

    IEnumerator DoDelayRelease()
    {
        yield return new WaitForSeconds(releaseDelay);
        smash(releaseTime, releaseHeight);
    }

    // Update is called once per frame
    void Start()
    {
        Smash();
    }
}
