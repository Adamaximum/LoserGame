using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerState;
    // 0 = Alive
    // 1 = Heaven
    // 2 = Hell

    public float playerSpeed = 0.2f;

    float horizontalInput;

    public float jumpVelocity = 2;

    public bool jumpCheck;

    public float fallMultiplier = 2.5f;

    Rigidbody2D playerRB;

    SpriteRenderer playerSR;
    public Color alive;
    public Color heaven;
    public Color hell;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        playerSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();

        if(playerState == 0)
        {
            playerSR.color = alive;
        }
        else if (playerState == 1)
        {
            playerSR.color = heaven;
        }
        else if (playerState == 2)
        {
            playerSR.color = hell;
        }
    }

    void MovementInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -playerSpeed;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = playerSpeed;
        }
        else
        {
            horizontalInput = 0;
        }

        transform.position += new Vector3(horizontalInput, 0f, 0f);

        if (playerState == 0)
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && jumpCheck == true)
            {
                playerRB.velocity = Vector2.up * jumpVelocity;
            }
        }
        else if (playerState == 1)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerRB.velocity = Vector2.up * jumpVelocity;
            }
        }

        if (playerRB.velocity.y < 0)
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCheck = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCheck = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spikes")
        {
            playerState = 2;
        }
    }
}
