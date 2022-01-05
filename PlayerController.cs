using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // CONST SIZE ENERGY REQUIREMENT
    private const int ENERGY_REQUIREMENT = 500;
    private const float GROWTH_RATE = 1.1f;

    Rigidbody2D body;
    private float horizontal;
    private float vertical;
    private float runSpeed;
    
    private SpriteRenderer playerSprite;
    private Animator animator;
    private bool growing;

    private int currentEnergy = 0;
    public int level = 1;
    private int size = 1;

    void Start()
    {
        UpdateCameraSize();
        UpdateSpeed();
        body = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!growing)
        {
            // get keyboard input
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // check if we're moving
            if (horizontal != 0 || vertical != 0)
            {
                // flip the sprite in the direction of our movement
                if (horizontal < 0)
                {
                    playerSprite.flipX = true;
                }
                else if (horizontal > 0)
                {
                    playerSprite.flipX = false;
                }

                // just running
                animator.SetInteger("State", 1);
            }
            else
            {
                // standing still
                animator.SetInteger("State", 0);
            }
        }
    }

    private void FixedUpdate()
    {
        // use our rigidbody to move
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public void Eat(int calories)
    {
        // add the calories
        currentEnergy += calories;
        if (CanGrow())
        {
            StartCoroutine(Grow());
        }
    }

    bool CanGrow()
    {
        // check for next level of growth
        return currentEnergy >= ENERGY_REQUIREMENT * size;
    }

    IEnumerator Grow()
    {
        // stop moving
        Stop();
        growing = true;
        
        // start the animation
        animator.SetInteger("State", 2);
        yield return new WaitForSecondsRealtime(.1f);
        // midway through the animation, grow
        transform.localScale *= GROWTH_RATE;
        size++;
        UpdateCameraSize();
        UpdateSpeed();
        if (size % 10 == 0)
        {
            level++;
        }
        if (!CanGrow())
        {
            // increment size level and reset our energy
            currentEnergy = 0;
            yield return new WaitForSecondsRealtime(1f);
            growing = false;
        }
        else
        {
            StartCoroutine(Grow());
        }
    }

    void Stop()
    {
        horizontal = 0;
        vertical = 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Tree"))
        {
            SpriteRenderer spriteRend = col.transform.GetComponent<SpriteRenderer>();
            Collider2D col2D = col.transform.GetComponent<Collider2D>();

            switch (level)
            {
                case 1:
                    
                    spriteRend.color = new Color(0, 0, 0, .2f);
                    col2D.enabled = false;

                    StartCoroutine(ResetObstacle(col.gameObject, spriteRend, col2D));
                    break;
                default:
                    if (Input.GetKey(KeyCode.Space))
                    {
                        col.transform.rotation = new Quaternion(.6f, 0, 0, 1);
                        spriteRend.color = new Color(0, 0, 0, .2f);
                        col2D.enabled = false;
                    }
                    break;
            }
        }
    }

    IEnumerator ResetObstacle(GameObject obst, SpriteRenderer spriteRenderer, Collider2D collider2D)
    {
        bool reset = false;
        do
        {
            yield return new WaitForSeconds(.5f);
            if (Vector2.Distance(transform.position, obst.transform.position) >= .3f)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
                collider2D.enabled = true;
                reset = true;
            }
        } while (!reset);
    }

    void UpdateCameraSize()
    {
        Camera.main.orthographicSize = transform.localScale.x * 2;
    }

    void UpdateSpeed()
    {
        runSpeed = transform.localScale.x * 2f;
    }
}