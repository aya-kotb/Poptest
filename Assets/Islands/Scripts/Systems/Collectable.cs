using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
    CollectionManager collectionManager;
	// Use this for initialization
	void Start ()
    {
        collectionManager = GameObject.FindObjectOfType<CollectionManager>();
	}
	
	public void collectMe()
    {
        collectionManager.collectItem(gameObject);
    }
}
