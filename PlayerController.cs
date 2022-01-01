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
    float horizontal;
    private float vertical;
    private float runSpeed = 4.0f;
    
    private SpriteRenderer playerSprite;
    private Animator animator;
    private bool growing;

    private int currentEnergy;
    private int level;

    void Start()
    {
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
        CheckForGrowth();
    }

    void CheckForGrowth()
    {
        // check for next level of growth
        if (currentEnergy >= ENERGY_REQUIREMENT * level)
        {
            StartCoroutine("Grow");
        }
    }

    IEnumerator Grow()
    {
        // stop other operations
        Stop();
        growing = true;
        // start the animation
        animator.SetInteger("State", 2);
        yield return new WaitForSecondsRealtime(.1f);
        // midway through the animation, grow
        transform.localScale *= GROWTH_RATE;
        // increment size level and reset our energy
        level++;
        currentEnergy = 0;
        yield return new WaitForSecondsRealtime(.5f);
        growing = false;
    }

    void Stop()
    {
        horizontal = 0;
        vertical = 0;
    }
}