using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalController : MonoBehaviour
{
    private const int ANIM_MIN = 4;
    private const int ANIM_MAX = 8;
    private const float RUN_DIST = .2f;

    // variables that change across animals
    public float speed = 0;
    public int requiredLevel = 0, numCalories = 1000;
    public GameObject blood;

    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontal, vertical;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>().transform;
        AssignMove();
    }

    private void FixedUpdate()
    {
        // use our rigidbody to move
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void AssignMove()
    {
        if (Vector2.Distance(player.position, transform.position) > RUN_DIST)
        {
            // we're safe, don't run
            int moveX = Random.Range(0, 3);

            switch (moveX)
            {
                case 0:
                    // right
                    horizontal = speed;
                    break;
                case 1:
                    // left
                    horizontal = -speed;
                    break;
                case 2:
                    // idle X
                    horizontal = 0;
                    break;
            }

            int moveY = Random.Range(0, 3);

            switch (moveY)
            {
                case 0:
                    // right
                    vertical = speed;
                    break;
                case 1:
                    // left
                    vertical = -speed;
                    break;
                case 2:
                    // idle Y
                    vertical = 0;
                    break;
            }
        }
        else
        {
            // run
            Vector2 runDir = player.position - transform.position;
            horizontal = runDir.x * 3;
            vertical = runDir.y * 3;
        }

        if (horizontal != 0 || vertical != 0)
        {
            StartCoroutine(Action(1, Random.Range(ANIM_MIN, ANIM_MAX)));
        }
        else
        {
            // idle
            StartCoroutine(Action(0, Random.Range(1, 3)));
        }
    }

    IEnumerator Action(int animState, int length)
    {
        animator.SetInteger("State", animState);

        if (horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }

        yield return new WaitForSecondsRealtime(length);
        AssignMove();
    }

    public void SetData(float spd, int rqrdLevel, int cals)
    {
        speed = spd;
        requiredLevel = rqrdLevel;
        numCalories = cals;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            // is player
            if (playerController.level >= requiredLevel)
            {
                // we're eaten
                Instantiate(blood, transform.position, quaternion.identity);
                print(numCalories);
                playerController.Eat(numCalories);
                Destroy(gameObject);
            }
        }
    }
}