using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public float speed;
    public PlayerSprite ps_script; //Allowing you to refrence the GameObject that holds the PlayerSprite script in the level by dragging them into the refrence point in the inspector
    public Jump j_script; //Allowing you to refrence the GameObject that holds the Jump script in the level by dragging them into the refrence point in the inspector
    public ArrowRotation a_script; //Allowing you to refrence the GameObject that holds the ArrowRotation script in the level by dragging them into the refrence point in the inspector
    private float moveInput;
    private Rigidbody2D rb;
    public bool facingRight = true;
    public float checkRadius;
    public bool jumping;
    public float currentJumpAmount;
    private float interimJumpAmount;
    private float landedJumpAmount;

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Adding a refrence to the rigid body located on the player gameobject
        jumping = false; //Set the player at the beginning to not currently be jumping
        j_script.aiming = false; //Setting the aiming to off at start just so the player doesn't immediately get confused by the jumping
        a_script.ArrowShow = false; //Setting the arrow to not show on the screen when game start
        ps_script.facingRightTrueFalse = facingRight; //Cross refrencing from this script to the PlayerSprite one which holds the flip function
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    //private void Update()
    //{
    //    Debug.Log("Prick"); (Angry debugging)
    //}

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    private void FixedUpdate() //Running all of the functions listed below that need to have constant checks every tick
    {
        AimingIfs();
        StopMovement();
        JumpingIfs();
        CharacterFacing();
        Collision();
        Debug.Log("Jumping:" + jumping + "Aiming:" + j_script.aiming);
        Debug.Log("Current jump amount" + currentJumpAmount);
        //Debug.Log(rb.gravityScale); (Test to see if the gravity on the RB (RigidBody) was changing
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public void Jump(Vector2 launchDir, float LaunchForce) //This function is ran from the Jump script by calling on its LaunchPlayer function
    {
        rb.AddForce(launchDir * LaunchForce); //Adding force to the player who is now in a state of zero gravity in the direction that the arrow is aiming
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void StopMovement() // Stops all player motion if aiming
    {
        if (jumping == true && j_script.aiming == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll; //Completely freezes the player on the spot until they jump at which point aiming becomes fales and the else condition kicks in
            // rb.velocity = Vector2.Zero (Old idea to just halt velocity however the above means that outside forces cannot move the player while aiming)
        }
        else 
        {
            rb.constraints = RigidbodyConstraints2D.None; //Sets the player character to have full mobility including rotation in any direction
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;// removing rotation as it sort of defeats the point of the flip function to a certain extent and makes the aiming and jumping alot more unnecissarilly convoluted
        }
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void AimingIfs() //Used for storing the if statements that involves the aiming boolean from the Jump script to see if the player is currently aiming in order to jump
    {
        if (j_script.aiming == true) //Checking if player has aiming on meaning they have either chained a jump or they have initiated aiming on the ground
        {
            
            a_script.ArrowShow = true; // Setting the Arrow to be visible in the scene by setting ArrowShow boolean form ArrowRotation script to true
            
            if (a_script.collidedObjectArrow.GetComponent<Renderer>().bounds.Intersects(a_script.Arrow.GetComponent<Renderer>().bounds) )
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("you can't jump into a wall from the same wall...");
                }
            }
            else// If there are no dangers or major obstacles in players way when thet press the spacebar then they will begin the jumping sequence
            {
                Debug.Log("Else if working");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Deku");
                    j_script.LaunchPlayer();//Calling LaunchPlayer function which gathers the necessary information needed to run the Jump function within this script
                }
               
            }
        }
        else if (j_script.aiming == false) //In the instance where the aiming boolean is false run the below script
        {
            
            a_script.ArrowShow = false; // Turning of arrow visibility
            if (Input.GetKeyDown(KeyCode.Space) && jumping == false && rb.velocity.y== 0) //Checking that the player has no downward or upward momentum and is not currently jumping before they can begin to aim
            {
                j_script.aiming = true; // If player is currently stationary and not aiming or jumping and presses the spacebar then aiming boolean will be set to true leading to the above if statement
                jumping = true; //When player begins aiming the jumping boolean will be set to true which will allow the gravity scale to change and stop the player from using WASD
            }
        }
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void JumpingIfs() //Used for storing the if statements that involve the jumping boolean from this script that states if player is currently in a jumping pattern
    {
        if (jumping != true)
        {
            rb.gravityScale = 1; // When player is not jumping the gravity will be at its normal state
            moveInput = Input.GetAxisRaw("Horizontal"); //Gets horizontal input from either WASD key layout or controller and sets the float moveInput to that value
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); // Sets velocity of the rigidbody to be the moveInput set above multiplied by the set speed float (Horizontal direction) and the current velocity in the y direction
            currentJumpAmount = 0;
        }
        else if (jumping == true)
        {
            rb.gravityScale = 0; // If player is currently aiming to jump or in the process of jumping then gravity will be turned off but only for the player
        }
    }

 //************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void CharacterFacing()
    {
        if (facingRight == false && moveInput > 0) // Check if player is moving right
        {
            ps_script.flip(); // Activate the flip function in the Flip script to make player face right
        }
        else if (facingRight == true && moveInput < 0) // Check if player is moving left
        {
            ps_script.flip(); // Activate the flip function in the Flip script to make player face left
        }
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void Collision() //This handles the collision with the objects the player can launch of such as wall and ones they cannot that kill their momentum, in this case the floor, these are organised by using tags which can be found in the inspector
    {
        if (jumping == true && j_script.aiming != true)
        {
            if (ps_script.collidedObject.tag == "LaunchingObject") // Checking if object that player collided with is listed as one of the LaunchingObjects
            {
                Debug.Log("Did it");
                j_script.aiming = true; // If that is the case then the player can launch of that object and continue their jump, turn on aiming arrow
                ps_script.collidedObject = null; //Setting the collided object in the PlayerSprite Script to nothing after it has been determined which object has been hit
                landedJumpAmount = currentJumpAmount; //Keeping track of the landed jump amount which is how many jumps have been landed on the Launching objects, this serves no purpose but to be entertaining to print in the debug

                StartCoroutine(CheckOfInterim()); // Running a Co-routine in order to give the player a timer before the jumping automatically stops to encourage faster gameplay and response times, they must jump before the timer runs out
            }
            if (ps_script.collidedObject.tag == "StoppingObject" && currentJumpAmount > 1) // Checking if object player collides with is listed as a one that they should stop on
            {
                Debug.Log("End my suffering");   
                jumping = false; // Sets jumping to false thus putting the player in basic movement mode
                ps_script.collidedObject = null;//Setting the collided object in the PlayerSprite Script to nothing after it has been determined which object has been hit
            }
        }
    }
    //************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    IEnumerator CheckOfInterim()
    {
        interimJumpAmount = currentJumpAmount; //Setting the interim that will be used to compare the current jump amount to the one form before the delay
        yield return new WaitForSeconds(.1f); //Delay of two seconds
        if (currentJumpAmount == interimJumpAmount)
        {
            yield return new WaitForSeconds(.1f); //Delay of two seconds
            if (currentJumpAmount == interimJumpAmount)
            {
                yield return new WaitForSeconds(.1f); //Delay of two seconds
                if (currentJumpAmount == interimJumpAmount)
                {
                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                    if (currentJumpAmount == interimJumpAmount)
                    {
                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                        if (currentJumpAmount == interimJumpAmount)
                        {
                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                            if (currentJumpAmount == interimJumpAmount)
                            {
                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                if (currentJumpAmount == interimJumpAmount)
                                {
                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                    if (currentJumpAmount == interimJumpAmount)
                                    {
                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                        if (currentJumpAmount == interimJumpAmount)
                                        {
                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                            if (currentJumpAmount == interimJumpAmount)
                                            {
                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                if (currentJumpAmount == interimJumpAmount)
                                                {
                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                    if (currentJumpAmount == interimJumpAmount)
                                                    {
                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                        if (currentJumpAmount == interimJumpAmount)
                                                        {
                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                            if (currentJumpAmount == interimJumpAmount)
                                                            {
                                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                if (currentJumpAmount == interimJumpAmount)
                                                                {
                                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                    if (currentJumpAmount == interimJumpAmount)
                                                                    {
                                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                        if (currentJumpAmount == interimJumpAmount)
                                                                        {
                                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                            if (currentJumpAmount == interimJumpAmount)
                                                                            {
                                                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                if (currentJumpAmount == interimJumpAmount)
                                                                                {
                                                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                    if (currentJumpAmount == interimJumpAmount)
                                                                                    {
                                                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                        if (currentJumpAmount == interimJumpAmount)
                                                                                        {
                                                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                            if (currentJumpAmount == interimJumpAmount)
                                                                                            {
                                                                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                if (currentJumpAmount == interimJumpAmount)
                                                                                                {
                                                                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                    if (currentJumpAmount == interimJumpAmount)
                                                                                                    {
                                                                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                        if (currentJumpAmount == interimJumpAmount)
                                                                                                        {
                                                                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                            if (currentJumpAmount == interimJumpAmount)
                                                                                                            {
                                                                                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                if (currentJumpAmount == interimJumpAmount)
                                                                                                                {
                                                                                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                    if (currentJumpAmount == interimJumpAmount)
                                                                                                                    {
                                                                                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                        if (currentJumpAmount == interimJumpAmount)
                                                                                                                        {
                                                                                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                            if (currentJumpAmount == interimJumpAmount)
                                                                                                                            {
                                                                                                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                if (currentJumpAmount == interimJumpAmount)
                                                                                                                                {
                                                                                                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                    if (currentJumpAmount == interimJumpAmount)
                                                                                                                                    {
                                                                                                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                        if (currentJumpAmount == interimJumpAmount)
                                                                                                                                        {
                                                                                                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                            if (currentJumpAmount == interimJumpAmount)
                                                                                                                                            {
                                                                                                                                                yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                                if (currentJumpAmount == interimJumpAmount)
                                                                                                                                                {
                                                                                                                                                    yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                                    if (currentJumpAmount == interimJumpAmount)
                                                                                                                                                    {
                                                                                                                                                        yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                                        if (currentJumpAmount == interimJumpAmount)
                                                                                                                                                        {
                                                                                                                                                            yield return new WaitForSeconds(.1f); //Delay of two seconds
                                                                                                                                                            jumping = false; // After delay is finished the jumping boolean is set to false
                                                                                                                                                            Debug.Log("Failure 2");
                                                                                                                                                            currentJumpAmount = 0;
                                                                                                                                                            j_script.aiming = false; // After delay is finished the aiming boolean in the Jump script is set to false
                                                                                                                                                        }
                                                                                                                                                    }
                                                                                                                                                }
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}