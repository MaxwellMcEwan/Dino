using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    // visual is for the opaque placement sprite
    public GameObject visual, actual;
    SpriteRenderer visualSprite;
    Color valid, invalid;
    Vector3 mousePosition;
    public bool active = false;
    public bool authorized = true;

    // Start is called before the first frame update
    void Start()
    {
        // create color coding for valid and invalid buildings
        valid = new Color(1, 1, 1, .7f);
        invalid = new Color(1, 0, 0, .7f);
    }

    // Update is called once per frame
    void Update()
    {
        // if we're building something manage the visuals
        if (active)
        {
            ManageBuild();
        }
    }

    public void BeginBuild(GameObject newVisual, GameObject newActual)
    {
        // begin the build and spawn visuals, save building data
        active = true;
        actual = newActual;
        visual = Instantiate(newVisual, new Vector3(0, 0, 0), Quaternion.identity);
        visualSprite = visual.GetComponent<SpriteRenderer>();
    }

    void ManageBuild()
    {
        // move the temporary visual to the mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        visual.transform.position = mousePosition;

        // show through color if you can build on the current area
        if (authorized)
        {
            visualSprite.color = valid;
            // if the mouse is clicked and the position is open, build away
            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuild();
            }
        }
        else
        {
            visualSprite.color = invalid;
        }
    }

    void PlaceBuild()
    {
        // spawn the building and get ready for next assignment
        Instantiate(actual, visual.transform.position, Quaternion.identity);
        active = false;
    }
}