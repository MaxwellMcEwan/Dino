using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // CONST SIZE ENERGY REQUIREMENT
    private const int ENERGY_REQUIREMENT = 1000;
    private const float GROWTH_RATE = 1.05f;
    private const int TREE_CAL = 100;

    // associated with our movement
    Rigidbody2D body;
    private float horizontal, vertical, runSpeed;
    
    // childrens components
    private SpriteRenderer playerSprite;
    private Animator animator;
    
    // are we in a growing state
    private bool growing, alive = true;

    // our current stats
    private int currentEnergy = 0, size = 1;
    public int level = 1, totalScore = 0;

    // EXTRAS
    public Sprite deathSprite;
    private UIManager uiMan;
    
    void Start()
    {
        // update speed according to our size
        UpdateSpeed();
        
        // get needed components
        body = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        uiMan = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        // if we're not growing, get the players movement
        if (!growing)
        {
            // get keyboard input
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // check if we're moving, to control animation
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

                // running animation
                animator.SetInteger("State", 1);
            }
            else
            {
                // standing still, set idle animation
                animator.SetInteger("State", 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            // use our rigidbody to move
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }
    }

    public void Eat(int calories)
    {
        // add the calories
        currentEnergy += calories;
        totalScore += calories;
        
        uiMan.UpdateScore(calories, currentEnergy, ENERGY_REQUIREMENT * size);
        
        // if we can grow, began the process
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
        
        uiMan.UpdateScore(0, 0, ENERGY_REQUIREMENT * size);
        
        // our size changed so update our speed
        UpdateSpeed();
        
        // if our size has grown by 10, update our level
        if (size % 10 == 0)
        {
            level++;
        }
        
        // check if we have enough calories to grow again
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
        // set our horizontal and vertical to zero
        horizontal = 0;
        vertical = 0;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        // if we collide with a tree
        if (col.transform.CompareTag("Tree"))
        {
            // get their components that we need
            SpriteRenderer spriteRend = col.transform.GetComponent<SpriteRenderer>();
            Collider2D col2D = col.transform.GetComponent<Collider2D>();

            switch (level)
            {
                case 1:
                    // disable their collider and change their appearance
                    spriteRend.color = new Color(1, 1, 1, .2f);
                    col2D.enabled = false;
                    
                    // begin timer to reset them
                    StartCoroutine(ResetObstacle(col.gameObject, spriteRend, col2D));
                    break;
                    
                case 2:
                    // we can't eat trees or fit through them, awkward
                    break;
                
                default:
                    // we can't walk in between trees anymore, but we can destroy them
                    if (Input.GetKey(KeyCode.Space))
                    {
                        // change their appearance, disable their collider
                        col.transform.rotation = new Quaternion(.6f, 0, 0, 1);
                        spriteRend.color = new Color(0, 0, 0, .2f);
                        col2D.enabled = false;
                        
                        // set their new layer so they don't appear in front of us
                        col.gameObject.layer = LayerMask.NameToLayer("Background");
                        
                        Eat(TREE_CAL);
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

    void UpdateSpeed()
    {
        runSpeed = transform.localScale.x * 2f;
    }

    public IEnumerator Killed()
    {
        animator.enabled = false;
        playerSprite.sprite = deathSprite;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        animator.enabled = true;
        transform.position = new Vector3(-1221, -1212, 0);
        FindObjectOfType<GameEvents>().PlayerDied(totalScore);
        alive = false;
        Time.timeScale = 1;
    }
}