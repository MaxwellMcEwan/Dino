using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool food;
    public int numCalories;
    public int healthRating;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            // give the item to the player
            col.transform.GetComponent<PlayerController>().Eat(numCalories);
            // destroy the item
            Destroy(this.gameObject);
        }
    }
}
