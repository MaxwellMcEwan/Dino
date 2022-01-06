using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int calories;
    public float carbs, protein, fat, satFat, transFat;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            // give the item to the player
            col.transform.GetComponent<PlayerController>().Eat(calories);
            // destroy the item
            Destroy(this.gameObject);
        }
    }

    public void SetData(int cal, float carb, float prot, float fatContent, float saturFat)
    {
        calories = cal;
        carbs = carb;
        protein = prot;
        fat = fatContent;
        satFat = saturFat;
        transFat = fat - saturFat;
    }
}
