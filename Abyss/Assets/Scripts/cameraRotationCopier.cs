using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotationCopier : MonoBehaviour {
    public float rotationAmountPerGravChange = 45;
    private Vector2 current;
    private Quaternion rotationAmount;
	// Use this for initialization
	void Start () {
        current = Physics2D.gravity;
        rotationAmount.eulerAngles = new Vector3(0, 0, rotationAmountPerGravChange);
	}
	
	// Update is called once per frame
	void Update () {
        if(Physics2D.gravity != current)
        {
            //determine change, use cross product to determine direction the gravity rotated, then apply the rotationAmount change in that direction
            transform.rotation *= (Vector3.Cross(current, Physics2D.gravity).z > 0) ? rotationAmount : Quaternion.Inverse(rotationAmount);
            current = Physics2D.gravity;
        }
	}
}