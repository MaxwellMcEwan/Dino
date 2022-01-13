using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalController : MonoBehaviour
{
    private const int ANIM_MIN = 4, ANIM_MAX = 8;
    private const float RUN_DIST = .2f;

    // variables that change across animals
    public float speed = 0;
    public int requiredLevel = 0, numCalories = 1000;
    public bool predator = false;
    
    public GameObject blood;

    // our components
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // movement variables
    private float horizontal, vertical;

    // to keep track of player distance
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // get all the components we need 
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>().transform;
        
        // begin the assign move -> action loop
        AssignMove();
    }

    private void FixedUpdate()
    {
        // use our rigidbody to move
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void AssignMove()
    {
        // if the player is far we don't need to run
        if (Vector2.Distance(player.position, transform.position) >= RUN_DIST)
        {
            if (transform.position.x < 0 || transform.position.x > GlobalVariables.SIZE || transform.position.y > GlobalVariables.SIZE || transform.position.y < 0)
            {

                if (transform.position.x < 0)
                {
                    // player is off map left
                    horizontal = 1;
                }
                else if (transform.position.x > GlobalVariables.SIZE)
                {
                    // player is off map right
                    horizontal = -1;
                }

                if (transform.position.y > GlobalVariables.SIZE)
                {
                    // player is off map above
                    vertical = -1;
                }
                else if (transform.position.y < 0)
                {
                    // player is off map below
                    vertical = 1;
                }
            }
            else
            {
                // we're safe, don't run
                // assign a random move for our x axis
                int moveX = Random.Range(-1, 2);
                horizontal = moveX * speed;

                // assign random move for our y axis
                int moveY = Random.Range(-1, 2);
                vertical = moveY * speed;
            }
        }
        else
        {
            // run from player
            // make a triangle and take the two sides to get a direction
            Vector2 runDir = player.position - transform.position;
            // multiplied by speed
            horizontal = runDir.x * speed;
            vertical = runDir.y * speed;
        }

        if (horizontal != 0 || vertical != 0)
        {
            // if we aren't idle, animate motion
            StartCoroutine(Action(1, Random.Range(ANIM_MIN, ANIM_MAX)));
        }
        else
        {
            // idle
            StartCoroutine(Action(0, Random.Range(ANIM_MIN, ANIM_MAX)));
        }
    }

    IEnumerator Action(int animState, int length)
    {
        // begin whichever animation should play
        animator.SetInteger("State", animState);

        // if we need to flip our sprite, do it
        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        // wait
        yield return new WaitForSecondsRealtime(length);
        
        // get next move
        AssignMove();
    }

    public void SetData(float spd, int rqrdLevel, int cals, int pred)
    {
        // get data from animal spawner for our stats
        speed = spd;
        requiredLevel = rqrdLevel;
        numCalories = cals;
        if (pred == 1)
        {
            predator = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        // if the collision is with the player
        if (col.transform.CompareTag("Player"))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            // if the player is the required level to eat us
            if (playerController.level >= requiredLevel)
            {
                // we're eaten, spawn blood, add calories, destroy us
                Instantiate(blood, transform.position, quaternion.identity);
                playerController.Eat(numCalories);
                Destroy(gameObject);
            }
            else
            {
                if (predator)
                {
                    StartCoroutine(playerController.Killed());
                }
            }
        }
    }
}