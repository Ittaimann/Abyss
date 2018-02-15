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
        Physics2D.IgnoreLayerCollision(14, 15, true);//square and squareOutline layers do not interact
        Physics2D.IgnoreLayerCollision(17, 18, true);//triangle and triangleOutline
        Physics2D.IgnoreLayerCollision(20, 21, true);//circle and circleOutline

        Physics2D.IgnoreLayerCollision(23, 15, true);//Player and squareOutline
        Physics2D.IgnoreLayerCollision(23, 18, true);//Player and triangleOutline
        Physics2D.IgnoreLayerCollision(23, 21, true);//Player and circleOutline

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("A collision was entered.\n");
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if(this.gameObject.layer == 16/*SquareTrigger*/ && other.gameObject.layer == 16/*SquareTrigger*/)
        {
            // Debug.Log("Testing a Square Collision.\n     " + transform.rotation.eulerAngles.z + " vs " + Mathf.Abs(transform.rotation.eulerAngles.z - other.transform.rotation.eulerAngles.z) % 90);
            float toTest = Mathf.Abs(transform.rotation.eulerAngles.z - other.transform.rotation.eulerAngles.z) % 90;
            toTest = (toTest > 45) ? toTest - 90 : toTest;
           // Debug.Log("\ntoTest = " + Mathf.Abs(toTest));
            if (Mathf.Abs(toTest) < RotationalLeniency)
            {
                Debug.Log("Square Match Identified");
                gameManager.removeSquareOutline(transform);
            }
        }
        else if (this.gameObject.layer == 19/*TriangleTrigger*/ && other.gameObject.layer == 19/*TriangleTrigger*/)
        {
            // Debug.Log("Testing a Triangle Collision.\n     " + transform.rotation.eulerAngles.z + " vs " + other.transform.rotation.eulerAngles.z);
            if (Mathf.Abs(transform.rotation.eulerAngles.z - other.transform.rotation.eulerAngles.z) < RotationalLeniency)
            {
                Debug.Log("Triangle Match Identified");
                gameManager.removeTriangleOutline(transform);
            }
        }
        else if (this.gameObject.layer == 22/*CircleTrigger*/ && other.gameObject.layer == 22/*CircleTrigger*/)
        {
            Debug.Log("Circle Match Identified");
            gameManager.removeCircleOutline(transform);
        }
    }
}
