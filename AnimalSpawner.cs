using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AnimalSpawner : MonoBehaviour
{
    private const int GLOBAL_NUM = 400, BIRDS_TO_FLOCK = 5;
    private WorldGenerator worldGenerator;
    private int animalCount = 0;

    public GameObject bird0;
    
    private string[] details = new string[] {"Speed", "RequiredLevel", "Calories", "Predator"};
    
    //define a list
    public static List <GameObject> animals = new List<GameObject>();

    Dictionary<string, Dictionary<string, float>> animalData = new Dictionary<string, Dictionary<string, float>>();

    public void SetWorld()
    {
        worldGenerator = GetComponent<WorldGenerator>();

        ReadAnimalDataIn();
        
        Object[] subListAnimals = Resources.LoadAll("Animals\\AnimalObjects", typeof(GameObject));
        foreach (Object subListAnimal in subListAnimals) 
        {
            // convert the objects into game objects and add them to our permanent list
            GameObject animal = (GameObject)subListAnimal;
            animals.Add(animal);
            animalCount++;
        }

        for (int i = 0; i < GLOBAL_NUM; i++)
        {
            SpawnAnimal();
        }
    }

    void SpawnAnimal()
    {
        // choose random item to spawn from list
        GameObject spawnAnimal = animals[Random.Range(0, animalCount)];
        if (animalData.ContainsKey(spawnAnimal.name))
        {
            AnimalController spawnDetails = spawnAnimal.GetComponent<AnimalController>();
            
            spawnDetails.SetData(animalData[spawnAnimal.name]["Speed"],
                Mathf.RoundToInt(animalData[spawnAnimal.name]["RequiredLevel"]),
                Mathf.RoundToInt(animalData[spawnAnimal.name]["Calories"]),
                Mathf.RoundToInt(animalData[spawnAnimal.name]["Predator"]));
        }

        // use world generators spawn function to put item on the map
        worldGenerator.SpawnItem(spawnAnimal, worldGenerator.world.transform);
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

            for (int i = 0; i < 4; i++)
            {
                animalDetails.Add(details[i], float.Parse(dataValues[i + 1]));
            }
            
            animalData.Add(dataValues[0], animalDetails);
        }
    }

    public void SpawnFlocks(int boardSize, int numFlocks, Transform parent)
    {
        for (int i = 0; i < numFlocks; i++)
        {
            int r = Random.Range(0, boardSize);
            int c = Random.Range(0, boardSize);

            for (int j = 0; j < BIRDS_TO_FLOCK; j++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-.99f, .99f), Random.Range(-.99f, .99f), 0);
                GameObject newSpawn = Instantiate(bird0, new Vector3(c, r, 0) + randomOffset, Quaternion.identity);
                newSpawn.transform.SetParent(parent);
            }
        }
    }
    
}
