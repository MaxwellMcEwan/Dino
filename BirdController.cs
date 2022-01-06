using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdController : MonoBehaviour
{
    private const float FLY_SPEED = 1.0f;
    
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;

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
    
    private void FixedUpdate()
    {
        // use our rigidbody to move
        body.velocity = new Vector2(horizontal * FLY_SPEED, vertical * FLY_SPEED);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player") && !scared)
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

        }
    }
}
