using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    private Rigidbody2D body; //Getting the rigidbody of the Player gameObject
    public ArrowRotation a_script; //Getting a refrence to the external script used to rotate the arrow 
    public PlayerController p_Script; //Getting a refrence to the player controller script that is on the Player gameObject in order to cross refrence and update variables and functions
    public Transform launchDirection; //The aiming direction which is an empty game object that is placed attatched to the arrow so it rotates with it but is significantly farther ahead 
    public float launchForce = 4000f; //Force the player is launched at, way to high break game but can be changed dynamically in the inspector
    public bool aiming; //Creating the boolean that is used to determine if the player can jump or not
    public PlayerSprite ps_script;

    //************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void Start() //Called before beginning of the first frame not in use in this script
    {

    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    void Update() //Called once per frame not in use in this script
    {
     
    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public void LaunchPlayer() //Function that is used to get all the necessary factors in launching the player character
    {
        aiming = false; //Setting aiming to false as this function is only called when the player has aimed and then chosen where they wish to propel
        ps_script.animator.SetBool("IsJumping", false);
        Debug.Log("Kill me");
        float launchDirX = launchDirection.position.x; //Extracting the X Direction from the empty Aiming gameObject mentioned prior
        float launchDirY = launchDirection.position.y; //Extracting the Y Direction from the empty Aiming gameObject mentioned prior
        float posX = transform.position.x; // Getting the current X position of player at time of jump
        float posY = transform.position.y; // Getting the current Y position of player at time of jump

        //Launch direction is the difference between our destination and our current position
        Vector2 launchDir = new Vector2(launchDirX - posX, launchDirY - posY); //This is used to direct where we will be shot along the aim of the arrow
        p_Script.currentJumpAmount = p_Script.currentJumpAmount + 1;
        p_Script.Jump(launchDir, launchForce); //Running the Jump script in the player controller adding in two variables from this script
    }
}
