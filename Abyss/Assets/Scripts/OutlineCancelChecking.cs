using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineCancelChecking : MonoBehaviour {
    public GameManager gameManager;
    public float RotationalLeniency = 0.0f;
    // Use this for initialization
    void Start () {
        if (RotationalLeniency == 0.0f)
            RotationalLeniency = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerStay2D(Collider2D other)
    {
        if(this.tag == "Square" && other.tag == "Square")
        {
            float toTest = Mathf.Abs(transform.rotation.eulerAngles.z - other.transform.rotation.eulerAngles.z) % 90;
            toTest = (toTest > 45) ? toTest - 90 : toTest;
            //Debug.Log("AngleDiff = " + Mathf.Abs(toTest));
            if (Mathf.Abs(toTest) < RotationalLeniency)
            {
                Debug.Log("Square Match Identified");
                //maybe tell game manager here that a square outline is going to dissapear
                //or tell every square  so that they can delete themselves when that num reaches 0
                gameManager.removeSquareOutline(transform);
            }
        }
        else if (this.tag == "Triangle" && other.tag == "Triangle")
        {
            Debug.Log(other.name);
            if (Mathf.Abs(transform.rotation.eulerAngles.z - other.transform.rotation.eulerAngles.z) < RotationalLeniency)
            {
                Debug.Log("Triangle Match Identified");
                gameManager.removeTriangleOutline(transform);
            }
        }
    }
}
