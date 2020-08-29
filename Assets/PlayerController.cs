using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float speed = 5f,
                  jumpForce = 10f;
    private enum States { Onground, Running, Jumping, Falling};
    private States currentState = States.Onground; 
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        float dy = Input.GetAxis("Horizontal");
        float dx = Input.GetAxis("Vertical");
        if (dy < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (dy > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        if (dx > 0 && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        StateSwitch();
    }

    private void StateSwitch()
    {   
        if (rb.velocity.y > 0)
        {
            currentState = States.Jumping;
        }
        else if (rb.velocity.y < 0)
        {
            currentState = States.Falling;
        }
        else
        {
            currentState = States.Onground;
        }

        if (rb.velocity.x != 0 && rb.velocity.y == 0)
        {
            currentState = States.Running;
        }
        else if(rb.velocity.y == 0)
        {
            currentState = States.Onground;
        }
        Debug.Log(currentState);
        anim.SetInteger("currentState", (int)currentState);
    }
}
