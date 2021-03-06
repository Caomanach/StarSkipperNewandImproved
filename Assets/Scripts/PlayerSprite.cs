﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public PlayerController p_Script;
    public bool facingRightTrueFalse;
    public GameObject collidedObject = null;
    public Animator animator;

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("launch", false);
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    // Update is called once per frame
    void Update()
    {
        p_Script.facingRight = facingRightTrueFalse;
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public void flip() // Function that will flip the players perspective depending on which way they are travelling
    {
        facingRightTrueFalse = !facingRightTrueFalse; //When funciton run the facingRightTrueFalse boolean is switched to it's opposite
        Vector3 Scaler = transform.localScale;//A new vector3 is made by getting the local scale of the PlayerSprite
        Scaler.x *= -1; //Making the X value of the Scaler be timesed by -1 so that the local scale can switch
        transform.localScale = Scaler; //Flipping the sprite by reversing its X scale
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    private void OnCollisionEnter2D(Collision2D collision) //Checking which 2D object that the collision box of the Player Sprite collided with and setting it so it can be passed for the check in the PlayerController Script
    {
        collidedObject = collision.gameObject;//Setting the object collided with into a usable, callable variable
        if (collidedObject.tag == "StoppingObject")
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public void RotateWhenInAir()
    {
        if (collidedObject.tag == "LaunchingObjectLeft")
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
        }
        else if (collidedObject.tag == "LaunchingObjectRight")
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
        }
        else if (collidedObject.tag == "LaunchingObjectUp")
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
        }
    }
}
