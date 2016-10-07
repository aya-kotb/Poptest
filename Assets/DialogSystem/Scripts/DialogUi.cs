using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogUi : MonoBehaviour
{
    [SerializeField]
    GameObject dialogTemplate, continueButton, rightPortrait, leftPortrait;
    [SerializeField]
    Camera rightCam, leftCam;
    [SerializeField]
    Text rightName, leftName;
    [SerializeField]
    DialogLayout layout;
    public AnimationData portraitAnimation, bubbleAnimation;

    public float endDelay;

    const string PLAYER_NAME = "Player";
    // Use this for initialization
    public GameObject CreateDialog(DialogData data, Dialog dialog)
    {
        // if speaker is not specified ("") they are the gameobject dialog is attached to
        // else go find the gameobject with the name of data.speaker
        GameObject speaker = data.speaker == "" ? dialog.gameObject : GameObject.Find(data.speaker);

        if(speaker == null)
        {
            Debug.Log("could not find character named: " + data.speaker);
            return null;
        }

        // are we in scene or in the special dialog popup? grab the template for that case and make it
        GameObject dialogBubble = GameObject.Instantiate(dialogTemplate);
        dialogBubble.transform.SetParent(dialogTemplate.transform.parent, false);

        RectTransform dialogPosition = dialogBubble.transform as RectTransform;

        AnimationData dialogAnim = new AnimationData(bubbleAnimation.animation, 
            new Vector3(bubbleAnimation.start.x, -layout.bottom), 
            new Vector3(bubbleAnimation.end.x, -layout.bottom), 
            bubbleAnimation.duration);//animate dialogBubble right to left

        AnimationData portraitAnim = new AnimationData(portraitAnimation.animation, 
            portraitAnimation.start, portraitAnimation.end, portraitAnimation.duration);//animate portrait right to left

        bool left = data.speaker == "";

        GameObject portrait;
        Camera cam;
        Text name;

        //color bubbles if they got a color they want to be
        Color color = speaker.GetComponent<Dialog>().dialogColor;
        dialogBubble.GetComponent<DialogTail>().UpdateTail(!left, color);

        if (!left)//right
        {
            dialogPosition.anchoredPosition = dialogAnim.start; // start off to the right
            portrait = rightPortrait;
            cam = rightCam;
            name = rightName;
        }
        else//left
        {
            // reverse animations
            dialogAnim.start = new Vector3(-dialogAnim.start.x, dialogAnim.start.y);
            dialogAnim.end = new Vector3(-dialogAnim.end.x, dialogAnim.end.y);
            portraitAnim.start = new Vector3(-portraitAnim.start.x, portraitAnim.start.y);
            portraitAnim.end = new Vector3(-portraitAnim.end.x, portraitAnim.end.y);

            dialogPosition.anchoredPosition = dialogAnim.start;// start off to the left
            portrait = leftPortrait;
            cam = leftCam;
            name = leftName;
        }

        Animate(dialogBubble, dialogAnim);//prepare bubble to animate

        string userName = "Hamburger Hamburger";//get user name from profile eventually

        string displayName = speaker.name == PLAYER_NAME ? userName : speaker.GetComponent<Dialog>().displayName;

        if (displayName != name.text)// if there is a new character
        {
            RectTransform portraitPosition = portrait.transform as RectTransform;
            if (portraitPosition.anchoredPosition.x != portraitAnim.start.x)// if you are not off screen slide off first then swap
            {
                AnimationData reverse = new AnimationData(portraitAnim.animation, portraitAnim.end, portraitAnim.start, portraitAnim.duration);
                Animate(portrait, reverse, delegate { SwapPortraits(dialogBubble, data, dialog, name, displayName, cam, speaker, portrait, portraitAnim); });
            }
            else // swap right away
                SwapPortraits(dialogBubble, data, dialog, name, displayName, cam, speaker, portrait, portraitAnim);
        }
        else // if the character is already on screen show there dialog
            EnableDialogBubble(dialogBubble, data, dialog);

        return dialogBubble;
    }
    //swap portraits then slide on screen
    void SwapPortraits(GameObject dialogBubble, DialogData data, Dialog dialog, Text name, string displayName, Camera cam, GameObject speaker, GameObject portrait, AnimationData portraitAnim)//set up portrait
    {
        name.text = displayName;

        FollowTarget follow = cam.GetComponent<FollowTarget>();
        follow.target = speaker.transform;
        Vector2 portraitOffset = speaker.GetComponent<Dialog>().portraitOffset;
        follow.offset = new Vector3(portraitOffset.x, portraitOffset.y, -5);

        Animate(portrait, portraitAnim, delegate { EnableDialogBubble(dialogBubble, data, dialog); });
    }

    void EnableDialogBubble(GameObject dialogBubble, DialogData data, Dialog dialog)
    {
        DialogBubble bubble = dialogBubble.GetComponent<DialogBubble>();
        bubble.dialogData = data;
        bubble.dialog = dialog;
        bubble.text.text = data.dialog;

        dialogBubble.SetActive(true);
    }

    void Animate(GameObject uiElement, AnimationData template, UnityAction onComplete = null)
    {
        TweenAnimation animation = uiElement.AddComponent<TweenAnimation>();
        animation.duration = template.duration;
        animation.SetAnimatioinData(template);
        if (onComplete != null)
            animation.onComplete.AddListener(onComplete);
    }

    public void DialogComplete(DialogBubble bubble, DialogData next)
    {
        if (next == null)
        {
            gameObject.GetComponentInChildren<ScrollRect>().enabled = true;// allow you to scroll through conversation
            continueButton.SetActive(true);//allow player to close dialog
        }
    }

    public void closeUI()
    {
        continueButton.SetActive(false);// or continue to spam closeUi

        //slide windows off screen
        Animate(rightPortrait, new AnimationData(portraitAnimation.animation, portraitAnimation.end, portraitAnimation.start, portraitAnimation.duration));

        Animate(leftPortrait, new AnimationData(portraitAnimation.animation, portraitAnimation.end, portraitAnimation.start * -1, portraitAnimation.duration), delegate { StartCoroutine( PortraitsRemoved()); });
    }

    IEnumerator PortraitsRemoved()
    {
        float delay = 0;
        while(delay < endDelay)
        {
            delay += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // remove all dialog bubbles
        DialogBubble[] bubbles = GameObject.FindObjectsOfType<DialogBubble>();
        for (int i = bubbles.Length - 1; i >= 0; i--)
        {
            DialogBubble bubble = bubbles[i];
            bubble.dialog.RemoveBubble(bubble);
        }
        // deactivate ui

        leftName.text = rightName.text = "";

        gameObject.GetComponentInChildren<ScrollRect>().enabled = false;// dont let players scroll while ui is closing

        gameObject.SetActive(false);
    }
}