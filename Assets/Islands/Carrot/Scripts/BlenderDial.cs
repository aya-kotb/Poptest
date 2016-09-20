using UnityEngine;

public class BlenderDial:MonoBehaviour
{
    private bool isDragging = false;

    void Start()
    {
        SetTornado(0);
    }

    void Update()
    {
        if(!isDragging)
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D collider = gameObject.GetComponent<Collider2D>();
                
                if(collider.OverlapPoint(ray))
                {
                    isDragging = true;
                }
            }
            else
            {
                UpdateDial();
            }
        }
        else
        {
            if(!Input.GetMouseButton(0))
            {
                isDragging = false;
                UpdateDial();
            }
            else
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 screenPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z - Camera.main.transform.position.z));

                gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2((screenPos.y - transform.position.y), (screenPos.x - transform.position.x)) * Mathf.Rad2Deg, Vector3.forward);
            }
        }
    }

    private void UpdateDial()
    {
        float z = gameObject.transform.rotation.eulerAngles.z;
        if((int)z != 200)
        {
            z += 25 * Time.deltaTime;
            gameObject.transform.rotation = Quaternion.AngleAxis(z, Vector3.forward);

            if((z > 325 && z < 360) || (z > 0 && z < 35))
            {
                SetTornado(4);
            }
            else if(z > 35 && z < 95)
            {
                SetTornado(3);
            }
            else if(z > 95 && z < 150)
            {
                SetTornado(2);
            }
            else if(z > 150 && z < 180)
            {
                SetTornado(1);
            }
            else if(z > 180 && z < 325)
            {
                SetTornado(0);
                if(z > 200 && z < 325)
                {
                    gameObject.transform.rotation = Quaternion.AngleAxis(200, Vector3.forward);
                }
            }
        }
    }

    private void SetTornado(int setting)
    {
        Transform blender = gameObject.transform.parent.FindChild("blender");
        if(blender)
        {
            Transform section = blender.FindChild("blender1");
            for(uint index = 2; section; ++index)
            {
                Vector3 scale = section.localScale;
                scale.x = (float)(setting + (setting * index * 0.1));
                section.localScale = scale;
                section = blender.FindChild("blender" + index);
            }
        }
    }
}