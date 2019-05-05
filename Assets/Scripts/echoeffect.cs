using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoeffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float timebtwspawns;
    public float starttimebtwspawns;

    public GameObject echo;
	// Update is called once per frame
	void Update () {
        if (timebtwspawns <= 0)
        {
            // spawn echo game object
            Instantiate(echo, transform.position, Quaternion.identity);
            timebtwspawns = starttimebtwspawns;
        } else
        {
            timebtwspawns -= Time.deltaTime;
        }
		
	}
}
