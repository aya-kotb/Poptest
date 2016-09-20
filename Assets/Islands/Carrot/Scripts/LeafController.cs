using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class LeafController : MonoBehaviour 
{
    public GameObject leafPrefab;
    public Vector3[] spawnPoints;
    private float timeLeft;

	// Use this for initialization
	void Start() 
    {
        timeLeft = Random.Range(2.0f, 8.0f);
	}
	
	// Update is called once per frame
	void Update() 
    {
        timeLeft -= Time.deltaTime;
        // Make another leaf fall and reset the timer
        if(timeLeft < 0)
        {
            CreateNewLeaf();
            timeLeft = Random.Range(15.0f, 20.0f);
        }
	}

    void CreateNewLeaf()
    {
        // find location of new leaf
        int randSpot = Mathf.RoundToInt(Random.Range(0.0f, spawnPoints.Length-1));
        Vector3 spawnLoc = spawnPoints[randSpot];
        Vector3 leafSpot = new Vector3(spawnLoc.x, spawnLoc.y, -1.1f);

        // create new leaf
        GameObject newLeaf = (GameObject)Instantiate(leafPrefab, leafSpot, Quaternion.identity);
        newLeaf.transform.rotation = Quaternion.AngleAxis(spawnLoc.z, Vector3.forward);

        // rescale so they have random sizes
        float randScale = Random.value * -1 + 0.75f;
        newLeaf.transform.localScale += new Vector3(randScale, randScale, 0.0f);
    }
}