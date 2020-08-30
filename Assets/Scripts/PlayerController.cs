using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /***********************************************************************/
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;



    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float speed = 5f,
                  jumpForce = 10f;
    [SerializeField]
    private Text CherryCount;


    /***********************************************************************/
    private int cherries = 0;
    private enum States { Onground, Running, Jumping, Falling };
    private States currentState = States.Onground;


    /***********************************************************************/
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        /***********************************************************************/
        /***********************************************************************/
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
    }



    /***********************************************************************/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            cherries++;
            CherryCount.text = cherries.ToString();
        }
    }
    private void Movement()
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
        else if (System.Math.Abs(rb.velocity.x) < 1)
        {
            currentState = States.Onground;
        }

        if (System.Math.Abs(rb.velocity.x) < 1 && rb.velocity.y == 0)
        {
            currentState = States.Onground;
        }
        else if (rb.velocity.x != 0 && rb.velocity.y == 0)
        {
            currentState = States.Running;
        }
        else if (rb.velocity.y == 0)
        {
            currentState = States.Onground;
        }
        anim.SetInteger("currentState", (int)currentState);
    }
}
