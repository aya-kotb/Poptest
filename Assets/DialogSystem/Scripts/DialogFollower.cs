using UnityEngine;
using System.Collections;
[RequireComponent(typeof(DialogBubble),typeof(FollowTarget))]
public class DialogFollower : MonoBehaviour
{
    DialogBubble bubble;
    FollowTarget follow;
    // Use this for initialization
    void Start ()
    {
        bubble = GetComponent<DialogBubble>();
        follow = GetComponent<FollowTarget>();

        GameObject speaker = bubble.dialogData.speaker == "" ? bubble.dialog.gameObject : GameObject.Find(bubble.dialogData.speaker);

        if(speaker)
        {
            follow.target = speaker.transform;
            bubble.transform.position = speaker.transform.position + follow.offset;
        }
        else
        {
            Debug.Log("could not find character named " + bubble.dialogData.speaker);
        }
    }
}
