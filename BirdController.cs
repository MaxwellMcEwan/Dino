using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdController : MonoBehaviour
{
    private const float FLY_SPEED = 1.0f, SELF_DESTRUCT = 30, FLY_DIST = .5f;
    private const int NUM_CALORIES = 150, REQ_LEVEL = 1;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;

    public GameObject blood;

    // birds won't act like other animals so they need a separate script
    private Transform player;

    private bool scared = false;

    private float horizontal, vertical;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        if (Vector2.Distance(player.position, transform.position) <= FLY_DIST && !scared)
        {
            // set fly animation to play
            animator.SetInteger("State", 1);
            scared = true;
            horizontal = Random.Range(-1, 2);
            vertical = Random.Range(-1, 2);

            if (horizontal < 0)
            {
                spriteRenderer.flipX = true;
            } else if (horizontal == 0)
            {
                horizontal = 1;
            }

            gameObject.layer = LayerMask.NameToLayer("Foreground");
            spriteRenderer.sortingLayerName = "Foreground";
            
            Destroy(gameObject, SELF_DESTRUCT);
        }
    }

    private void FixedUpdate()
    {
        // use our rigidbody to move
        body.velocity = new Vector2(horizontal * FLY_SPEED, vertical * FLY_SPEED);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            // is player
            if (playerController.level >= REQ_LEVEL)
            {
                // we're eaten
                Instantiate(blood, transform.position, Quaternion.identity);
                playerController.Eat(NUM_CALORIES);
                Destroy(gameObject);
            }
        }
    }
}
