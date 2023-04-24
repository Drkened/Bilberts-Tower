using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    private float moveInput;
    public bool isGrounded;
    private Rigidbody2D rb;
    public LayerMask groundMask;
    public SpriteRenderer spriteRenderer;


    public PhysicsMaterial2D bounceMat, normalMat;
    public bool canJump;
    public float jumpValue = 0.0f;

    public Sprite jumpSprite;
    public Sprite walkSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        canJump = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame


    void Update()
    {
        if(!isGrounded)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = jumpSprite;
        }else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = walkSprite;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput < 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        if (moveInput > 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
    

        if(jumpValue == 0.0f && isGrounded)
        {
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);
        }
        
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(0.9f, 0.4f), 0f, groundMask);

        if(jumpValue >0)
        {
            rb.sharedMaterial = bounceMat;
        }else
        {
            rb.sharedMaterial = normalMat;
        }
        if(Input.GetKey("space") && isGrounded && canJump)
        {
            jumpValue += 0.1f;
        }

        if(Input.GetKeyDown("space") && isGrounded && canJump)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            walkSpeed = 7.5f;
        }


        if(jumpValue >= 30 && isGrounded)
        {
            float tempx = moveInput * walkSpeed;
            float tempy = jumpValue;
            rb.velocity = new Vector2(tempx, tempy);
            Invoke("ResetJump", 0.05f);
        }

        if(Input.GetKeyUp("space"))
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(moveInput * walkSpeed, jumpValue);
                jumpValue = 0.0f;
                canJump = true;
            }

            walkSpeed = 5.0f;
        }
     }

     void ResetJump()
     {
        jumpValue = 0.0f;
     }

}