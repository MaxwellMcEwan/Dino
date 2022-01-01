using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private const int GLOBAL_NUM = 100;

    private WorldGenerator worldGenerator;
    
    //define a list
    public static List <GameObject> items = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        worldGenerator = GetComponent<WorldGenerator>();
        
        // load in all the items from the resources
        Object[] subListObjects = Resources.LoadAll("Items", typeof(GameObject));
        foreach (Object subListObject in subListObjects) 
        {
            // convert the objects into game objects and add them to our permanent list
            GameObject item = (GameObject)subListObject;
            items.Add(item);
        }

        // initial global spawn of items
        for (int i = 0; i < GLOBAL_NUM; i++)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    { 
        // choose random item to spawn from list
        GameObject spawnItem = items[Random.Range(1, 3)];
        // use world generators spawn function to put item on the map
        worldGenerator.SpawnItem(spawnItem);
    }
}
