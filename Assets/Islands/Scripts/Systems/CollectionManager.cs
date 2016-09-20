using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CollectionManager : MonoBehaviour
{
    [SerializeField]
    Text display;
    [SerializeField]
    string descriptor;
    //SceneController settings;
    public GameObject[] allCollectableGroups;
    string[] collectedItems;
    GameObject collectables;
    string collected;
	// Use this for initialization
    /*
	void Start ()
    {
        if (settings == null)
            settings = GameObject.FindObjectOfType<SceneController>();
        settings.onSceneLoaded.AddListener(sceneLoaded);
        collectedItems = new string[settings.levels.Length];

        UpdateCollectionDisplay();
	}

    void UpdateCollectionDisplay()
    {
        if (display != null)
        {
            display.text = descriptor == null ? "" : descriptor + ": ";
            display.text += "" + itemsCollected + "/" + itemsToCollect;
        }
    }
	
    void sceneLoaded()
    {
        if (collectables != null)
            GameObject.Destroy(collectables);
        
        GameObject group = allCollectableGroups[settings.Level];
        // not every scene may have carrots
        if (group != null)
        {
            collectables = GameObject.Instantiate(group) as GameObject;
            collected = collectedItems[settings.Level];
            if (collected == null || collected == "")
                collected = "";
            else
            {
                foreach(Transform t in collectables.transform)
                {
                    if(collected.IndexOf(t.name) >= 0)
                    {
                        GameObject.Destroy(t.gameObject);//get rid of already collected carrots
                    }
                }
            }
        }
    }

    public int itemsCollected
    {
        get
        {
            int totalItemsCollected = 0;
            for (int i = 0; i < collectedItems.Length; i++)
            {
                string temp = collectedItems[i];
                if (temp != null)
                {
                    totalItemsCollected += temp.Split(',').Length;
                }
            }
            return totalItemsCollected;
        }
    }

    public int itemsToCollect
    {
        get
        {
            int total = 0;
            for (int i = 0; i < allCollectableGroups.Length; i++)
            {
                GameObject group = allCollectableGroups[i];
                if (group != null)
                {
                    total += group.transform.childCount;
                }
            }
            return total;
        }
    }
    */
	public void collectItem(GameObject item)
    {
        if (collected == "")
            collected += item.name;
        else
            collected += "," + item.name;

        //collectedItems[settings.Level] = collected;
        GameObject.Destroy(item);

        //UpdateCollectionDisplay();
    }
}