using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private const int GLOBAL_NUM = 100;
    private WorldGenerator worldGenerator;
    
    //define a list
    public static List <GameObject> items = new List<GameObject>();

    Dictionary<string, Dictionary<string, object>> itemData = new Dictionary<string, Dictionary<string, object>>();
    
    // Start is called before the first frame update
    void Start()
    {
        ReadItemDataIn();
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

    void ReadItemDataIn()
    {
        StreamReader strReader = new StreamReader("C:\\Users\\maxwe\\OneDrive\\Documents\\Unity Projects\\MyGame\\Assets\\ItemData.csv");
        bool endOfFile = false;
        while (!endOfFile)
        {
            string data = strReader.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = data.Split(',');
            
            Dictionary<string, object> itemDetails = new Dictionary<string, object>();
            itemDetails.Add("Calories", dataValues[1]);
            itemDetails.Add("Carbohydrates", dataValues[2]);
            itemDetails.Add("Protein", dataValues[3]);
            itemDetails.Add("Fat", dataValues[4]);
            itemDetails.Add("SatFat", dataValues[5]);

            itemData.Add(dataValues[0], itemDetails);
        }
    }
}
