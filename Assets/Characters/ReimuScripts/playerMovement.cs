using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // controlling Reimu's type of movement
    public Rigidbody2D playerBody; // the collider is a box collider for now
    public float playerSpeed;
    public float normalSpeed;
    public float slowSpeed;
    public bool playerCanMove;


    // storing movement data
    private float moveX;
    private float moveY;
    private Vector2 movement;


    [Header("HitBox Dynamics")]
    public GameObject hitbox;
    public GameObject outline;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        ProcessPlayerInputs();    
    }

    private void FixedUpdate()
    {
        playerBody.velocity = new Vector2(movement.x * playerSpeed, movement.y * playerSpeed);
    }

    private void ProcessPlayerInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
        //if (hitbox.activeInHierarchy && outline.activeInHierarchy)
        //{
         
        //}

        if (Input.GetKey(KeyCode.LeftShift) && playerCanMove)
        {
            playerSpeed = slowSpeed;
         
        }
        else if(Input.GetKey(KeyCode.LeftShift) && playerCanMove == false)
        {
            playerSpeed = 0;
        }
        else if(playerCanMove)
        {
            playerSpeed = normalSpeed;
        }
        else
        {
           
            
            playerSpeed = 0;
        }
    }
}
