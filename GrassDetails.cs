using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassDetails : MonoBehaviour
{
    private const int NUM_DETAILS = 10000;
    
    //define a list
    public static List <GameObject> grassMarks = new List<GameObject>();
    
    public void SpawnDetails(int boardSize, Transform parent)
    {
        Object[] subListObjects = Resources.LoadAll("Environment\\GrassDetails", typeof(GameObject));
        foreach (Object subListObject in subListObjects) 
        {
            // convert the objects into game objects and add them to our permanent list
            GameObject mark = (GameObject)subListObject;
            print(mark.name);
            grassMarks.Add(mark);
        }

        for (int i = 0; i < NUM_DETAILS; i++)
        {
            int markIndex = Random.Range(0,10);
            if (markIndex >= 5)
            {
                markIndex = 0;
            }

            Vector3 randomOffset = new Vector3(Random.Range(-.99f, .99f), Random.Range(-.99f, .99f), 0);
            
            int r = Random.Range(0, boardSize);
            int c = Random.Range(0, boardSize);
            GameObject newSpawn = Instantiate(grassMarks[markIndex], new Vector3(c, r, 0) + randomOffset, Quaternion.identity);
            newSpawn.transform.SetParent(parent);
        }
    }
}
