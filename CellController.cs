using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private Rigidbody2D body;
    private bool waiting, pushed = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        body.gravityScale = UnityEngine.Random.Range(-.01f, .011f);
    }

    private void FixedUpdate()
    {
        // use our rigidbody to move
        if (transform.position.x < 0)
        {
            body.gravityScale = -.01f;
        } else if (transform.position.x > GlobalVariables.SIZE)
        {
            body.gravityScale = .01f;
        }

        if (!waiting && body.velocity == new Vector2() )
        {
            StartCoroutine(FlipGravityScale());
        }
    }

    public void Pop()
    {
        if (pushed)
        {
            // STUB
            print("popped");
            Destroy(gameObject);
        }
    }
    
    IEnumerator FlipGravityScale()
    {
        waiting = true;
        yield return new WaitForSeconds(3);
        if (body.velocity == new Vector2())
        {
            body.gravityScale *= -1;
        }
        waiting = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.otherCollider.CompareTag("Player"))
        {
            pushed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.otherCollider.CompareTag("Player"))
        {
            pushed = false;
        }
    }
}
