using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private const int REQ_LEVEL = 3;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Tree"))
        {
            Destroy(col.gameObject);
        }

        if (col.transform.CompareTag("Player"))
        {
            if (col.transform.GetComponent<PlayerController>().level >= REQ_LEVEL)
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .2f);
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}
