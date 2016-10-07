using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
public class DialogBubbleSizer : MonoBehaviour
{
    public float padding, minHeight;
    [SerializeField]
    Text textField;
    RectTransform rect;
    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float height = Mathf.Max(minHeight, textField.preferredHeight + padding * 2);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
    }
}