using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AnimalSpawner : MonoBehaviour
{
    private const int GLOBAL_NUM = 40;
    private WorldGenerator worldGenerator;

    public GameObject bird0;
    
    private string[] details = new string[] {"Speed", "RequiredLevel", "Calories"};
    
    //define a list
    public static List <GameObject> animals = new List<GameObject>();

    Dictionary<string, Dictionary<string, float>> animalData = new Dictionary<string, Dictionary<string, float>>();

    // Start is called before the first frame update
    void Start()
    {
        worldGenerator = GetComponent<WorldGenerator>();

        ReadAnimalDataIn();
        
        Object[] subListAnimals = Resources.LoadAll("Animals\\AnimalObjects", typeof(GameObject));
        foreach (Object subListAnimal in subListAnimals) 
        {
            // convert the objects into game objects and add them to our permanent list
            GameObject animal = (GameObject)subListAnimal;
            animals.Add(animal);
        }

        for (int i = 0; i < GLOBAL_NUM; i++)
        {
            SpawnAnimal();
        }
    }

    void SpawnAnimal()
    {
        // choose random item to spawn from list
        GameObject spawnAnimal = animals[Random.Range(0, 2)];
        if (animalData.ContainsKey(spawnAnimal.name))
        {
            AnimalController spawnDetails = spawnAnimal.GetComponent<AnimalController>();
            
            spawnDetails.SetData(animalData[spawnAnimal.name]["Speed"],
                Mathf.RoundToInt(animalData[spawnAnimal.name]["RequiredLevel"]),
                Mathf.RoundToInt(animalData[spawnAnimal.name]["Calories"]));
        }

        // use world generators spawn function to put item on the map
        worldGenerator.SpawnItem(spawnAnimal);
    }

    void ReadAnimalDataIn()
    {
        StreamReader strReader = new StreamReader("C:\\Users\\maxwe\\OneDrive\\Documents\\Unity Projects\\MyGame\\Assets\\Scripts\\AnimalData.csv");
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
            Dictionary<string, float> animalDetails = new Dictionary<string, float>();

            for (int i = 0; i < 3; i++)
            {
                animalDetails.Add(details[i], float.Parse(dataValues[i + 1]));
            }
            
            animalData.Add(dataValues[0], animalDetails);
        }
    }

    
}
