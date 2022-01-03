using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // CONSTANTS
    const int SIZE = 100;
    const int SMOOTHS = 7;
    const int FLORA_MULT = 4;

    // general world
    public GameObject world, water;
    GameObject plot;

    // bitMaps
    private int[,] bitMap = new int[SIZE, SIZE];
    private int[,] caveMap = new int[SIZE, SIZE];
    private int[,] floraMap = new int[SIZE * FLORA_MULT, SIZE * FLORA_MULT];

    // flora
    public GameObject tree1;

    // player
    public GameObject player;

    // cave system
    public GameObject underground, floor, wall;

    // Start is called before the first frame update
    void Start()
    {
        // general world
        bitMap = GenerateBitMap(SIZE);
        SmoothBitMap(bitMap, SIZE);
        PlotBitMap(bitMap, SIZE, water, 0, world);

        // flora
        floraMap = GenerateBitMap(SIZE * FLORA_MULT);
        SmoothBitMap(floraMap, SIZE * FLORA_MULT); 
        ApplyLandRestrictions();
        SmoothBitMap(floraMap, SIZE * FLORA_MULT);
        PlotFloraMap();

        // spawn player on land
        SpawnItem(player);

        // generate cave system
        //caveMap = GenerateBitMap(SIZE);
        //SmoothBitMap(caveMap, SIZE);
        //PlotBitMap(caveMap, SIZE, wall, floor, -1, underground);
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

    void PlotBitMap(int[,] mapToPlot, int size, GameObject item0, int level, GameObject parent)
    {
        // build game map from bitMap
        for (int r = 0; r < size; r++)
        {
            // every row
            for (int c = 0; c < size; c++)
            {
                // every box
                // find type
                if (mapToPlot[r, c] == 0)
                {
                    // place plot
                    GameObject newPlot = Instantiate(item0, new Vector3(c, r, level), Quaternion.identity);
                    newPlot.transform.parent = parent.transform;

                }
            }
        }
    }

    void ApplyLandRestrictions()
    {
        // set all water spaces to no fauna
        for (int r = 0; r < SIZE * FLORA_MULT; r++)
        {
            // every row
            for (int c = 0; c < SIZE * FLORA_MULT; c++)
            {
                // every box
                // find type
                if (floraMap[r, c] == 1)
                {
                    // check if the tree is over a water plot
                    if (bitMap[Mathf.FloorToInt((float) r / FLORA_MULT), Mathf.FloorToInt((float) c / FLORA_MULT)] == 0)
                    {
                        floraMap[r, c] = 0;
                    }
                }
            }
        }
    }

    void PlotFloraMap()
    {
        // build game map from bitMap
        for (int r = SIZE * FLORA_MULT - 1; r > -1; r--)
        {
            // every row
            for (int c = SIZE * FLORA_MULT - 1; c > -1; c--)
            {
                // every box
                // find type
                if (floraMap[r, c] == 1)
                {
                    // place plot
                    GameObject newFlora = Instantiate(tree1,
                        new Vector3((float) c / FLORA_MULT - ((float) FLORA_MULT / 10), (float) r / FLORA_MULT, -1),
                        Quaternion.identity);
                    newFlora.transform.parent = world.transform;
                }
            }
        }
    }

    public void SpawnItem(GameObject toSpawn)
    {
        bool spawned = false;
        do
        {
            int r = Random.Range(1, SIZE - 1);
            int c = Random.Range(1, SIZE - 1);
            if (bitMap[r, c] == 1 && floraMap[r * FLORA_MULT, c * FLORA_MULT] == 0)
            {
                Instantiate(toSpawn, new Vector3(c, r, 0), Quaternion.identity);
                spawned = true;
            }
        } while (!spawned);
    }
}