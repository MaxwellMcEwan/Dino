using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BloodCellMovement : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // use our rigidbody to move
            transform.Translate(new Vector3(BloodDirection.dirX,BloodDirection.dirY,0));
        }
    }
}

public static class BloodDirection
{
    public static float dirX = 0.001f;
    public static float dirY = 0;
}
