using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodMill : MonoBehaviour
{
  // CONSTANTS
  const float MINE_RADIUS = 2.5f;
  private const float DEG = 0.0349065850398f;
  private const float MINE_COOL = 0.01f;
  
  private int quad = 1;
  Vector2 mineAngle = new Vector2(1, 0);
  bool canMine;
  public LayerMask treeLayer;


  private Economy economy;
    // Start is called before the first frame update
    void Start()
    {
      // begin mining treess immediately and get needed components 
      StartCoroutine("MineTree");
      economy = FindObjectOfType<Economy>().GetComponent<Economy>();
    }

    // Update is called once per frame
    void Update()
    {
      // if we finished mining the last tree, begin a new operation
      if (canMine) {
        StartCoroutine("MineTree");
      }
    }

    IEnumerator MineTree() {
      canMine = false;
      yield return new WaitForSecondsRealtime(MINE_COOL); //Wait 1 second
      // mine a tree in a circle
      RaycastHit2D hit = Physics2D.Raycast(transform.position, mineAngle, MINE_RADIUS, treeLayer);

      // If it hits something...
      if (hit.collider != null)
      {
        if (hit.transform.tag == "Tree")
        {
          // harvest the tree
          hit.transform.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);
          hit.transform.GetComponent<BoxCollider2D>().enabled = false;
          economy.sellTree();
        }
      }

      // Prof. Bowen would be thrilled I used the unit circle
      switch (quad)
      {
        /*
         *  BREAKDOWN OF THE SWITCH STATEMENT
         *
         * This switch uses the current quadrant to move the x
         * and y around it in a circle. A ray is sent in the
         * direction of the point and destroys trees.
         * 
         */
        case 1:
          mineAngle += new Vector2(-DEG, DEG);
          if (mineAngle.x <= 0)
          {
            quad++;
          }
          break;
        case 2:
          mineAngle += new Vector2(-DEG, -DEG);
          if (mineAngle.x <= -1)
          {
            quad++;
          }
          break;
        case 3:
          mineAngle += new Vector2(DEG, -DEG);
          if (mineAngle.x >= 0)
          {
            quad++;
          }
          break;
        case 4:
          mineAngle += new Vector2(DEG, DEG);
          if (mineAngle.x >= 1)
          {
            quad = 1;
          }
          break;
      }
      canMine = true;
    }
}
