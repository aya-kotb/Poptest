using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogLayout : MonoBehaviour
{
    public float spacing = 0;
    ScrollRect scroll;
    RectTransform rect;
    RectTransform bounds;
    DialogBubble[] bubbles;

    public float targetTop;
    public RectTransform reference;

    public float bottom
    {
        get
        {
            return rect.sizeDelta.y;
        }
    }

    public DialogBubble lastBubble
    {
        get
        {
            if (bubbles.Length == 0)
                return null;
            return bubbles[bubbles.Length - 1];
        }
    }

    // Use this for initialization
    void Awake ()
    {
        scroll = GetComponentInParent<ScrollRect>();
        rect = transform as RectTransform;
        bounds = transform.parent.GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -spacing);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, -spacing);
    }
    
    // Update is called once per frame
    void Update ()
    {
        bubbles = GetComponentsInChildren<DialogBubble>();
        float y = -spacing;
        for(int i = 0; i < bubbles.Length; i++)
        {
            RectTransform pos = bubbles[i].GetComponent<RectTransform>();
            pos.anchoredPosition = new Vector2(pos.anchoredPosition.x, y);
            y -= pos.sizeDelta.y + spacing;
        }
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, -y);
        if(scroll.enabled)
        {
            return;
        }
        float anchorY = reference.position.y + targetTop - y;
        rect.position = new Vector2(rect.position.x, anchorY);
    }
}