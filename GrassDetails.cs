using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDetails : MonoBehaviour
{
    //define a list
    public static List <GameObject> groundDetails = new List<GameObject>();


    public Transform detailsParent;
    
    // amount of objects we loaded in
    private int numDetails;
    
    public void SpawnDetails(string path, int numSpawns)
    {
        // get a list of all available grass details to make
        Object[] subListObjects = Resources.LoadAll(path, typeof(GameObject));
        foreach (Object subListObject in subListObjects) 
        {
            // convert the objects into game objects and add them to our permanent list
            GameObject mark = (GameObject)subListObject;
            groundDetails.Add(mark);
            numDetails++;
        }

        // run this loop NUM_DETAILS amount of times
        for (int i = 0; i < numSpawns; i++)
        {
            // get a random index, it is out of range because I want there to be a higher chance of the default grass mark
            int markIndex = Random.Range(0, numDetails);
            
            // produce a random offset to our detail
            Vector3 randomOffset = new Vector3(Random.Range(-.99f, .99f), Random.Range(-.99f, .99f), 0);
            
            // select a random x, y
            int r = Random.Range(0, GlobalVariables.SIZE);
            int c = Random.Range(0, GlobalVariables.SIZE);
            
            // choose the object from the index, add the offset, and spawn the detail. Set the details parent
            GameObject newSpawn = Instantiate(groundDetails[markIndex], new Vector3(c, r, 0) + randomOffset, Quaternion.identity);
            newSpawn.transform.localScale = new Vector3(Random.Range(newSpawn.transform.localScale.x * .60f, newSpawn.transform.localScale.x * 1.2f), Random.Range(newSpawn.transform.localScale.x * .6f, newSpawn.transform.localScale.x * 1.2f), 1);
            newSpawn.transform.SetParent(detailsParent);
        }
    }
}
