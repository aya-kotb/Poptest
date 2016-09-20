using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{
    float time, duration, start, end, dif;
    public UnityEvent fadeComplete;
	// Use this for initialization
	void Start ()
    {
	    
	}

    void Update()
    {
        /*just for testing purposes
        if (Input.GetKeyDown(KeyCode.F))
        {
            fade(5,1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            fade(5,0);
        }
        */
    }

	public void fade(float duration = 1, float endAlpha = 1)
    {
        this.duration = duration;
        time = 0;
        end = endAlpha;
        StartCoroutine("DoFade");
    }

    IEnumerator DoFade()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
            start = renderers[0].material.color.a;
        else
            start = end;
        dif = end - start;

        bool shouldFade = dif != 0;
        
        while (shouldFade)
        {
            time += Time.deltaTime;
            shouldFade = (time < duration);
            if (!shouldFade)
                time = duration;
            
            foreach (Renderer childRend in renderers)
            {
                if (childRend)
                {
                    Color color = new Color(1, 1, 1, start + time / duration * dif);
                    childRend.material.color = color;
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        fadeComplete.Invoke();
    }
}