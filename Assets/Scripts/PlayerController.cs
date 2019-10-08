﻿using System.Collections;
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
    float verticalInput;

    [Header ("Jump Settings")]
    public float jumpVelocity = 2;
    public bool jumpCheck;
    public float fallMultiplier = 2.5f;

    Rigidbody2D playerRB;

    SpriteRenderer playerSR;

    [Header ("State Colors")]
    public Color alive;
    public Color heaven;
    public Color hell;

    public bool hellContact;

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
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        if (hellContact)
        {
            playerRB.velocity = new Vector2(0f, 0f);
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

        if (playerState == 2 && hellContact)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                verticalInput = -playerSpeed;
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                verticalInput = playerSpeed;
            }
            else
            {
                verticalInput = 0;
            }
        }
        else
        {
            verticalInput = 0;
        }

        transform.position += new Vector3(horizontalInput, verticalInput, 0f);

        if (playerState == 0) // Jumping is Normal
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && jumpCheck == true)
            {
                playerRB.velocity = Vector2.up * jumpVelocity;
            }
        }
        else if (playerState == 1) // Jumping is Infinite
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerRB.velocity = Vector2.up * jumpVelocity;
            }
        }
        if (playerState < 2) // Fall Multiplier is not active when Hell is active
        {
            if (playerRB.velocity.y < 0)
            {
                playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Collision Enter
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCheck = true;
        }
        if (collision.gameObject.tag == "Bullet" && playerState == 0)
        {
            playerState = 1;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // Collision Exit
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCheck = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Trigger Enter
    {
        if(collision.gameObject.tag == "Spikes" && playerState == 0)
        {
            playerState = 2;
            playerRB.velocity = new Vector2(0f, 0f);
        }

        if(collision.gameObject.name == "Resurrector")
        {
            playerState = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) // Trigger Stay
    {
        if (collision.gameObject.tag == "Ground" && playerState == 2)
        {
            playerRB.gravityScale = 0;
            hellContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // Trigger Exit
    {
        if (collision.gameObject.tag == "Ground" && playerState == 2)
        {
            if (playerState == 2)
            {
                playerRB.gravityScale = 1;
            }
            hellContact = false;
        }
    }
}
