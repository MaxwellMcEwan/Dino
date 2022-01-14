using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cell"))
        {
            print("Collided");
            other.gameObject.GetComponent<CellController>().Pop();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            print("Collided player");
            StartCoroutine(other.gameObject.GetComponent<PlayerController>().Killed());
        }
    }
}
