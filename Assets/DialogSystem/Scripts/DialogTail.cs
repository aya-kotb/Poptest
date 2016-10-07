using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogTail : MonoBehaviour
{
    public GameObject rightTail, leftTail;

    public void UpdateTail(bool right, Color color)
    {
        rightTail.SetActive(right);
        leftTail.SetActive(!right);
        if (color != Color.black && color.a > 0)
        {
            gameObject.GetComponent<Image>().color = color;
            leftTail.GetComponent<Image>().color = color;
            rightTail.GetComponent<Image>().color = color;
        }
    }
}