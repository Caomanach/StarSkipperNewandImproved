using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public bool ArrowShow;//Boolean used to determing if the arrow should be visible or not
    public GameObject Arrow; //Reference to the game object that's used to hold the arrow sprite in the right location around the player
    //Attempts at ray tracing
    public float offset;
    //public float rayDistance;
    //public LayerMask whatIsSolid;
    public PlayerController p_script; //Refrence to the PlayerController script which is added by dragging and dropping a refrence to the gameobject holding aforementioned scirpt in the scene into the Inspector and specifically the section that covers this script
    public PlayerSprite ps_script; // Refrence to the PlayerSprite script same as above
    public GameObject collidedObjectArrow; //Created an empty gameObject variable to be used to store which objects the arrow collides with while aiming to be refrenced in the PlayerController later
    //Trying to make the mouse be less responsive or turn of as a response to attempting to aim at a wall while on it leading to a glitch
    //public bool mouseInputOnorOff = true;

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    // Start is called before the first frame update
    void Start()
    {

    }

//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; //Getting the difference between the camera and the location of the mouse on the screen taking away the player's location on the screen 
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; //Creating a float variable to hold the Z rotator which will be used to rotate around the Player, Unity puts all angles in radians so it must be converted into degrees as seen here
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);// Rotating the arrow based on the mouse position using a Quaternion(?), no clue what it does but the Euler portion is used to ensure that it is given as degrees and not radians which gives more precission and better control to the player
//*************************************************MISTAKES********************************************************************************************************************************************************************************************************************************************************************************************************************************************
        //Debug.Log(transform.eulerAngles.z);

        //if (mouseInputOnorOff == true)
        //{
        //    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //    float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        //}
        //if (mouseInputOnorOff == false)
        //{

        //}

        //************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
        if (ArrowShow == true) //Checking to see if the arrow needs to be visible
        {
            Arrow.GetComponent<Renderer>().enabled = true; //Setting the arrow to be visible to the player
            GetComponent<Collider2D>().enabled = true; //Activating the arrows collision capsule so that collision events can occur when aiming leading to the refrence to the collisionArrowObject seen below
        }
        else if (ArrowShow == false) //if the ArrowShow variable happens to equal false then the collider is disabled so as not to cause too much collision issues
        {
            GetComponent<Collider2D>().enabled = false; //Disabling the collision
            Arrow.GetComponent<Renderer>().enabled = false; //Turning visibility on the arrow off
        }

//*********************************************************MISTAKES*****************************************************************************************************************************************************************************************************************************************************************************************************************************************
        //    if (collidedObjectArrow.tag == "LaunchingObject")
        //    {
        //        if (collidedObjectArrow.GetComponent<Renderer>().bounds.Intersects(Arrow.GetComponent<Renderer>().bounds))
        //        {
        //            Debug.Log("We are active");


        //            if (transform.eulerAngles.z < 135 & transform.eulerAngles.z > 90)
        //            {
        //                Debug.Log("Mad kING");
        //                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
        //                StartCoroutine(HittingEdgeOfAiming());
        //                if (collidedObjectArrow.GetComponent<Renderer>().bounds.Intersects(Arrow.GetComponent<Renderer>().bounds))
        //                {
        //                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //                    StartCoroutine(HittingEdgeOfAiming());
        //                }

        //            }
        //            else if (transform.eulerAngles.z > 180 & transform.eulerAngles.z < 225)
        //            {
        //                Debug.Log("Proof in the pudding");
        //                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
        //                StartCoroutine(HittingEdgeOfAiming());
        //            }
        //            else if (transform.eulerAngles.z > 270 & transform.eulerAngles.z < 315)
        //            {

        //                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
        //                StartCoroutine(HittingEdgeOfAiming());
        //            }
        //            else if (transform.eulerAngles.z < 45 & transform.eulerAngles.z > 0)
        //            {
        //                Debug.Log("Proof in the pudding");
        //                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //                StartCoroutine(HittingEdgeOfAiming());
        //            }
        //            else if (transform.rotation.z > .75 & transform.rotation.z < 180 || transform.rotation.z > -135 && transform.rotation.z < -90 || transform.rotation.z < 0 && transform.rotation.z > -45 || transform.rotation.z > 45 && transform.rotation.z < 90)
        //            {
        //                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 5);
        //            }
        //        }     
        //    }
        //    else if (collidedObjectArrow.tag == "StoppingObject")
        //    {
        //        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 5);
        //    }
        //    else
        //    {
        //        Physics2D.IgnoreCollision(collidedObjectArrow.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        //    }
        //}
    }
//************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    private void OnCollisionStay2D(Collision2D collision) //Getting a refrence to the object that the Arrow has collided with (the collider for the arrow is actually on it's rotator to make getting its refrence easier however it does rotate like the arrow so it is basically the arrows collider
    {
        collidedObjectArrow = collision.gameObject;// Setting the object that collided with the arrow to a variable to be refrenced in the PlayerController script to see if the jump chain can continue of it will stop depending on the surface landed on
    }
    //private void OnCollisionExit2D(Collision2D collision) //Running a function when the collision to the object in the last section ends
    //{
    //    collidedObjectArrow; //Setting object to null so that the player is unable to re-aim in the air
    //}

    //************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************************
    public void AimingFlip()
    {
        if(p_script.j_script.aiming == true)
        {
            if (ps_script.facingRightTrueFalse == false && transform.eulerAngles.z < 90 || ps_script.facingRightTrueFalse == false && transform.eulerAngles.z > 270)
            {
                Debug.Log("Flip from left to right");
                ps_script.flip();
            }
            if (ps_script.facingRightTrueFalse == true && transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
            {
                Debug.Log("Flip from right to left");
                ps_script.flip();
            }
        }
    }
}

//************************************************************MISTAKES****************************************************************************************************************************************************************************************************************************************************************************************************************************************
//IEnumerator HittingEdgeOfAiming()
//{
//    Debug.Log("mOUSEY WOUSEY");
//    mouseInputOnorOff = false;
//    yield return new WaitForSeconds(2.5f);
//    mouseInputOnorOff = true;

//}
//void Raycasting()
//{
//    RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Arrow.transform.position, 10000, whatIsSolid);
//    if (hitInfo.collider != null)
//    {
//        Debug.Log("Collided");
//        if (hitInfo.collider.CompareTag("LaunchingObject"))
//        {
//            Debug.Log("Arrow pointing at or through wall");
//        }
//        if (hitInfo.collider.CompareTag("StoppingObject"))
//        {
//            Debug.Log("Arrow pointing at floor");
//        }
//    }


//}
