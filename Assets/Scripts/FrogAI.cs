using System;
using Unity.Mathematics;
using UnityEngine;

public class FrogAI : Enemy
{
    private Rigidbody2D frogBody;
    private Collider2D coll;
    private enum States { Idle, Jumping, Falling };
    private States currentState = States.Idle;
    [SerializeField]
    private int speed = 2,
                jumpForce = 3;
    private bool IsFacingLeft = true;
    [SerializeField]
    private LayerMask ground,
                      enemy;
    [SerializeField]
    private RigidbodyType2D bestatic;

    protected override void Start()
    {
        base.Start();
        frogBody = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        StateSwitch();
    }

    private void artificialMovement()
    {
        if (IsFacingLeft)
        {
            if (frogBody.velocity.y == 0 )
            {
                frogBody.velocity = new Vector2(speed, jumpForce);
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else
        {
            if (frogBody.velocity.y == 0 )
            {
                frogBody.velocity = new Vector2(-speed, jumpForce);
                transform.localScale = new Vector2(1, 1);
            }
        }
        if (coll.IsTouchingLayers(enemy))
        {
            if (IsFacingLeft)
            {
                    frogBody.velocity = new Vector2(speed, frogBody.velocity.y);
                    transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                    frogBody.velocity = new Vector2(-speed, frogBody.velocity.y);
                    transform.localScale = new Vector2(1, 1);
            }

        }
    }

    private void StateSwitch()
    {
        if (frogBody.velocity.y > 0)
        {
            currentState = States.Jumping;
        }
        else if (frogBody.velocity.y < 0)
        {
            currentState = States.Falling;
        }
        else
        {
            currentState = States.Idle;
        }
        anim.SetInteger("Animation", (int)currentState);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyCollidesWallRight")
        {
            IsFacingLeft = true;
        }
        else if (collision.tag == "EnemyCollidesWallLeft")
        {
            IsFacingLeft = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (IsFacingLeft)
            {
                IsFacingLeft = false;
                frogBody.velocity = new Vector2(-speed*2, 10);
                transform.localScale = new Vector2(1, 1);

            }
            else
            {
                IsFacingLeft = true;
                frogBody.velocity = new Vector2(speed * 2, 10);
                transform.localScale = new Vector2(-1, 1);

            }
        }
        
    }
    private void Stop()
    {
        Debug.Log("Died");
        frogBody.velocity = new Vector2(0, 0);
        coll.isTrigger = true;
        frogBody.bodyType =  bestatic;
    }

}
