using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(DialogBubble))]
public class DialogLifeTime : MonoBehaviour
{
    DialogBubble bubble;
    float time,speed,lifeTime;
    bool complete;
    public bool spellOut;
    public float waitTime;

    // Use this for initialization
    void Start()
    {
        bubble = GetComponent<DialogBubble>(); 
        time = 0;
        speed = PlayerProfileLanguageExample.dialogSpeed;// eventually need to find it through some other reference
        lifeTime = bubble.dialogData.dialog.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (complete)
            return;

        time += Time.deltaTime * speed;

        if(time < lifeTime && spellOut)
            bubble.text.text = bubble.dialogData.dialog.Substring(0, (int)time);

        if (time > lifeTime + waitTime)
        {
            complete = true;
            bubble.complete();
        }
    }
}
