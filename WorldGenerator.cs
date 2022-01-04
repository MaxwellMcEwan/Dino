using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // CONSTANTS
    const int SIZE = 30;
    const int SMOOTHS = 7;
    const int FLORA_MULT = 4;
    
    // general world
    public GameObject world;
    
    // bitMaps
    private int[,] floraMap = new int[SIZE * FLORA_MULT, SIZE * FLORA_MULT];

    // flora
    public GameObject tree0, tree1, currTree;

    // player
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        // flora
        floraMap = GenerateBitMap(SIZE * FLORA_MULT);
        SmoothBitMap(floraMap, SIZE * FLORA_MULT); 
        PlotFloraMap();

        // spawn player on land
        SpawnItem(player);
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
        for (int r = SIZE * FLORA_MULT - 1; r > -1; r--)
        {
            // every row
            for (int c = SIZE * FLORA_MULT - 1; c > -1; c--)
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
                    newFlora.transform.localScale = new Vector3(Random.Range(2f, 2.6f), Random.Range(2f, 2.8f), 1);
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
            if (floraMap[r * FLORA_MULT, c * FLORA_MULT] == 0)
            {
                Instantiate(toSpawn, new Vector3(c, r, 0), Quaternion.identity);
                spawned = true;
            }
        } while (!spawned);
    }
}