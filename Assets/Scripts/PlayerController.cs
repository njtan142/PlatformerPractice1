using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /***********************************************************************/
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private AudioSource sound;

    [SerializeField]
    private AudioClip HurtSound, RunningSound, CollectSound;

    [SerializeField]
    private LayerMask ground;
    [SerializeField]
    private float speed = 5f,
                  jumpForce = 10f;
    [SerializeField]
    private Text CherryCount,
                 EnemiesKilledCount;


    /***********************************************************************/
    private int cherries = 0;
    private int enemiesKilled = 0;
    private enum States { Onground, Running, Jumping, Falling, Hurt };
    private States currentState = States.Onground;


    /***********************************************************************/
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        sound = GetComponent<AudioSource>();
        /***********************************************************************/
        /***********************************************************************/
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != States.Hurt)
        {
            Movement();
        }
        StateSwitch();
    }



    /***********************************************************************/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            cherries++;
            CherryCount.text = cherries.ToString();
            didCollect();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
            Debug.Log("Enemy:" + enemyRB.position);
            Debug.Log("Player:" + rb.position);

            if (currentState == States.Falling && enemyRB.position.y < rb.position.y)
            {
                enemy.JumpedOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                enemiesKilled++;
                EnemiesKilledCount.text = enemiesKilled.ToString();
            }
            else if (currentState == States.Jumping && enemyRB.position.y > rb.position.y)
            {
                enemy.JumpedOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                enemiesKilled++;
                EnemiesKilledCount.text = enemiesKilled.ToString();
            }
            else
            {
                currentState = States.Hurt;
                didHurt();
                enemyRB.velocity = new Vector2(0, 0);
                Debug.Log(enemyRB.velocity);
                if (collision.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-speed, jumpForce / 2);
                }
                else
                {
                    rb.velocity = new Vector2(speed, jumpForce / 2);
                }
            }
        }
    }
    private void Movement()
    {
        float dy = Input.GetAxis("Horizontal");
        float dx = Input.GetAxis("Jump");
        Debug.Log(dx);
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
        if (dx == 1 && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void StateSwitch()
    {
        if (currentState != States.Hurt)
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
                RunSound();
            }
            else if (rb.velocity.y == 0)
            {
                currentState = States.Onground;
            }
        }
        else
        {
            if (System.Math.Abs(rb.velocity.x) < 1)
            {
                currentState = States.Onground;
            }
        }
        anim.SetInteger("currentState", (int)currentState);
    }
    private void RunSound()
    {
        sound.clip = RunningSound;
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }
    private void FallSound()
    {
        sound.clip = RunningSound;
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }
    private void didHurt()
    {

        sound.clip = HurtSound;
            sound.Play();
    }
    private void didCollect()
    {

        sound.PlayOneShot(CollectSound, 1f);
    }
}
