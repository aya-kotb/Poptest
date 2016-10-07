using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class FlipImage : MonoBehaviour
{
    public bool flipX, flipY;
	// Use this for initialization
	void Start ()
    {
        Image image = GetComponent<Image>();
        Rect rect = image.sprite.rect;

        if(flipX)
        {
            rect.x += rect.width;
            rect.width = -rect.width;
        }
        if (flipY)
        {
            rect.y += rect.height;
            rect.height = -rect.height;
        }

        image.sprite = Sprite.Create(image.sprite.texture, rect, image.sprite.pivot);
    }
}
