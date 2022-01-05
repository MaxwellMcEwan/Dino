using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private const float RUN_SPEED = .4f;
    private const int ANIM_MIN = 4;
    private const int ANIM_MAX = 8;
    private const float RUN_DIST = .2f;

    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontal;
    private float vertical;

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
        body.velocity = new Vector2(horizontal * RUN_SPEED, vertical * RUN_SPEED);
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
                    horizontal = RUN_SPEED;
                    break;
                case 1:
                    // left
                    horizontal = -RUN_SPEED;
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
                    vertical = RUN_SPEED;
                    break;
                case 1:
                    // left
                    vertical = -RUN_SPEED;
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
            Vector2 runDir = transform.position - player.position;
            horizontal = runDir.x;
            vertical = runDir.y;
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
        animator.SetInteger("state", animState);

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
}