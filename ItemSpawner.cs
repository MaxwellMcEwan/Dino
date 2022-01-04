using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    private const int GLOBAL_NUM = 200;
    private WorldGenerator worldGenerator;

    private string[] details = new string[] {"Calories", "Carbohydrates", "Protein", "Fat", "SatFat"};
    
    //define a list
    public static List <GameObject> items = new List<GameObject>();

    Dictionary<string, Dictionary<string, float>> itemData = new Dictionary<string, Dictionary<string, float>>();
    
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
        if (itemData.ContainsKey(spawnItem.name))
        { 
            Item spawnDetails = spawnItem.GetComponent<Item>();
            spawnDetails.SetData(Mathf.RoundToInt(itemData[spawnItem.name]["Calories"]),
                itemData[spawnItem.name]["Carbohydrates"],
                itemData[spawnItem.name]["Protein"],
                itemData[spawnItem.name]["Fat"],
                itemData[spawnItem.name]["SatFat"]);
        }
        
        // use world generators spawn function to put item on the map
        worldGenerator.SpawnItem(spawnItem);
    }

    void ReadItemDataIn()
    {
        StreamReader strReader = new StreamReader("C:\\Users\\maxwe\\OneDrive\\Documents\\Unity Projects\\MyGame\\Assets\\Scripts\\ItemData.csv");
        bool endOfFile = false;
        // skip a line
        strReader.ReadLine();
        while (!endOfFile)
        {
            string data = strReader.ReadLine();
            if (data == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = data.Split(',');
            Dictionary<string, float> itemDetails = new Dictionary<string, float>();

            for (int i =0; i < 5; i++)
            {
                itemDetails.Add(details[i], float.Parse(dataValues[i + 1]));
            }
            
            itemData.Add(dataValues[0], itemDetails);
        }
    }
}
