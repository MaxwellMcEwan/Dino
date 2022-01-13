using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    // CONSTANTS
    private const int SMOOTHS = 7, FLORA_MULT = 4, NUM_ROCKS = 30, NUM_FLOCKS = 6;
    
    // general world
    public GameObject world;
    public Color groundColor = new Color((float)117/255, (float)154/255, 128/255, 1), microbeColor = new Color((float)1/255, (float)1/255, 1/255, 1);
    private Color currBackground;

    // bitMaps
    private int[,] floraMap = new int[GlobalVariables.SIZE * FLORA_MULT, GlobalVariables.SIZE * FLORA_MULT];

    // flora
    public GameObject tree0, tree1;
    private GameObject currTree;
    
    // rocks
    public GameObject singleRock, doubleRock;
    private GameObject currRock;

    // grass details
    private GrassDetails grassDetails;
    
    // player
    public GameObject player;
    
    // birds
    private AnimalSpawner animalSpawner;
    
    // change Between Scenes (defaulted to microbe)
    private bool randomizeSize = false;
    private string detailsPath = "Environment\\CellDetails";
    private int bgDetails = 7000;


    public void SpawnWorld()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                // microbe setup
                currBackground = microbeColor;
                break;
            case 2:
                // grass setup
                currBackground = groundColor;
                randomizeSize = true;
                detailsPath = "Environment\\GrassDetails";
                animalSpawner.SetWorld();
                bgDetails = 10000;
                break;
        }
        
        Camera.main.backgroundColor = currBackground;
        
        // spawn grass details
        grassDetails = GetComponent<GrassDetails>();
        grassDetails.SpawnDetails(detailsPath, bgDetails);

        // flora
        floraMap = GenerateBitMap(GlobalVariables.SIZE * FLORA_MULT);
        SmoothBitMap(floraMap, GlobalVariables.SIZE * FLORA_MULT); 
        PlotFloraMap();

        // spawn player on land
        SpawnItem(player);
        player = FindObjectOfType<PlayerController>().gameObject;

        // spawn birds
        animalSpawner = GetComponent<AnimalSpawner>();
        animalSpawner.SpawnFlocks(GlobalVariables.SIZE, NUM_FLOCKS, world.transform);

        // spawn rocks
        SpawnRocks();
        
    }

    private void Update()
    {
        TrackPlayer();
    }

    public void SpawnRocks()
    {
        for (int i = 0; i < NUM_ROCKS; i++)
        {
            int rockType = Random.Range(0, 2);
            switch (rockType)
            {
                case 0:
                    currRock = singleRock;
                    break;
                case 1:
                    currRock = doubleRock;
                    break;
            }
            
            SpawnItem(currRock, world.transform);
        }
    }
    
    int[,] GenerateBitMap(int size)
    {
        int[,] newBitMap = new int[size, size];
        // fill the array with values
        for (int r = 1; r < size - 1; r++)
        {
            // every row
            for (int c = 1; c < size - 1; c++)
            {
                // every box
                newBitMap[r, c] = Random.Range(0, 2);
            }
        }

        return newBitMap;
    }

    void SmoothBitMap(int[,] mapToSmooth, int size)
    {
        // smooth the bitMap
        int specialWeight;
        for (int s = 0; s < SMOOTHS; s++)
        {
            for (int r = 1; r < size - 1; r++)
            {
                // every row
                for (int c = 1; c < size - 1; c++)
                {
                    // every box
                    specialWeight = 0;
                    // for the 8 boxes surrounding the original
                    for (int br = -1; br < 2; br++)
                    {
                        for (int bc = -1; bc < 2; bc++)
                        {
                            // get the amount of grass blocks surrounding you
                            if (mapToSmooth[r + br, c + bc] == 1)
                            {
                                specialWeight++;
                            }
                        }
                    }

                    // switch to whichever plot type is most frequently found
                    if (specialWeight > 4)
                    {
                        mapToSmooth[r, c] = 1;
                    }
                    else
                    {
                        mapToSmooth[r, c] = 0;
                    }
                }
            }
        }
    }

    void PlotFloraMap()
    {
        // build game map from bitMap
        for (int r = GlobalVariables.SIZE * FLORA_MULT - 1; r > -1; r--)
        {
            // every row
            for (int c = GlobalVariables.SIZE * FLORA_MULT - 1; c > -1; c--)
            {
                // every box
                // find type
                if (floraMap[r, c] == 1)
                {
                    if (Random.Range(0,4) <= 2)
                    {
                        currTree = tree0;
                    }
                    else
                    {
                        currTree = tree1;
                    }
                    
                    // place plot
                    GameObject newFlora = Instantiate(currTree,
                        new Vector3((float) c / FLORA_MULT - ((float) FLORA_MULT / 10), (float) r / FLORA_MULT, -1),
                        Quaternion.identity);
                    if (randomizeSize)
                    {
                        newFlora.transform.localScale = new Vector3(Random.Range(newFlora.transform.localScale.x * 1.1f, newFlora.transform.localScale.x * 1.6f), Random.Range(newFlora.transform.localScale.x * 1.1f, newFlora.transform.localScale.x * 1.6f), 1);
                    }
                    newFlora.transform.parent = world.transform;
                }
            }
        }
    }

    public void SpawnItem(GameObject toSpawn, Transform parent = null)
    {
        bool spawned = false;
        do
        {
            int r = Random.Range(4, GlobalVariables.SIZE - 4);
            int c = Random.Range(4, GlobalVariables.SIZE - 4);
            Vector3 randomOffset = new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0);
            
            if (floraMap[r * FLORA_MULT, c * FLORA_MULT] == 0)
            {
                GameObject newSpawn = Instantiate(toSpawn, new Vector3(c, r, 0) + randomOffset, Quaternion.identity);
                if (parent != null)
                {
                    newSpawn.transform.SetParent(parent);
                }

                spawned = true;
            }
        } while (!spawned);
    }

    void TrackPlayer()
    {
        Vector2 pos = player.transform.position;
        
        if (pos.x < 0 || pos.x > GlobalVariables.SIZE || pos.y > GlobalVariables.SIZE || pos.y < 0)
        {
            Vector2 nearestValid = new Vector2(pos.x, pos.y);
            
            if (pos.x < 0)
            {
                // player is off map left
                nearestValid.x = 0;
            }
            else if (pos.x > GlobalVariables.SIZE)
            {
                // player is off map right
                nearestValid.x = GlobalVariables.SIZE;
            }
            if (pos.y > GlobalVariables.SIZE)
            {
                // player is off map above
                nearestValid.y = GlobalVariables.SIZE;
            }
            else if (pos.y < 0)
            {
                // player is off map below
                nearestValid.y = 0;
            }

            float colorVal = Vector2.Distance(pos, nearestValid) / 10;
            
            Camera.main.backgroundColor = currBackground - new Color(colorVal,colorVal, colorVal, 1);
        }
    }
}

public static class GlobalVariables
{
    public const int SIZE = 30;
}