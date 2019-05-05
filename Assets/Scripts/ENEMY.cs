using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY : MonoBehaviour {

    public LayerMask enemymask;
    public float speed = 1;
    Rigidbody2D mybody;
    Transform mytrans;
    float mywidth;


	// Use this for initialization
	void Start () {
        mytrans = this.transform;
        mybody = this.GetComponent<Rigidbody2D>();
        mywidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //check to see if theres ground in front of them

        Vector2 lineCastPos = mytrans.position - mytrans.right * mywidth;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down );

        bool isgrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemymask);
        //if no ground turn around
        if(!isgrounded)
        {
            Vector3 currot = mytrans.eulerAngles;
            currot.z += 180;
            mytrans.eulerAngles = currot;
        }

        //Always move forward
        Vector2 myvel = mybody.velocity;
        myvel.x = -mytrans.right.x *speed;
        mybody.velocity = myvel;

		
	}
}
