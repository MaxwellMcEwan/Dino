using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : MonoBehaviour
{
  private int treeValue = 10;
  public int money;
  public GameObject visMill, actMill;
  PlaceManager placeManager;


    // Start is called before the first frame update
    void Start()
    {
      placeManager = GetComponent<PlaceManager>();
      placeManager.BeginBuild(visMill, actMill);
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q) && !placeManager.active)
      {
        placeManager.BeginBuild(visMill, actMill);
      }
    }

    public void sellTree()
    {
      money += treeValue;
    }
}
