using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotationCopier : MonoBehaviour {
    public float rotationAmountPerGravChange = 45;
    public bool test = false;
    private List <string> blockLayers;
    private Vector2 current;
    private bool clockwise = true;
    private bool blocked = false;
    private Quaternion rotationAmount;
    private Quaternion targetRotation;
    private Quaternion lastRotation;
    private Quaternion blockedRotation;
    private int timer = 10;
    //private Quaternion temp;
	// Use this for initialization
	void Start () {
        blockLayers = new List <string>();
        if (this.gameObject.layer != 19)
            blockLayers.Add("Triangle");
        if (this.gameObject.layer != 16)
            blockLayers.Add("Square");
        if (this.gameObject.layer != 22)
            blockLayers.Add("Circle");
        
        targetRotation = transform.rotation;
        lastRotation = targetRotation;
        current = Physics2D.gravity;
        rotationAmount.eulerAngles = new Vector3(0, 0, rotationAmountPerGravChange);
	}
	
	// Update is called once per frame
	void Update () 
    {

        if(Physics2D.gravity != current)
        {
            blocked = false;
            clockwise = (Vector3.Cross(current, Physics2D.gravity).z > 0);
            //determine change, use cross product to determine direction the gravity rotated, then apply the rotationAmount change in that direction
            targetRotation *=  clockwise ? rotationAmount : Quaternion.Inverse(rotationAmount);
            current = Physics2D.gravity;
        }
        float rotationDifference;
        if (!blocked)
            rotationDifference = (transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z) % 360;
        else
            rotationDifference = (transform.rotation.eulerAngles.z - lastRotation.eulerAngles.z) % 360;
        if (rotationDifference < 0)
            rotationDifference += 360;
        clockwise = rotationDifference > 180;

        if (!blocked && (3 < Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z)))
        {
            //if(test)
              //  Debug.Log(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z);
            
            Quaternion target = Quaternion.Euler(0, 0, transform.rotation.z + (clockwise ? 3 : -3));
            transform.rotation *= target;// Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4F);
        }
        else
        {
            if (blocked && (3 < Mathf.Abs(transform.rotation.eulerAngles.z - lastRotation.eulerAngles.z)))
            {
                Quaternion target = Quaternion.Euler(0, 0, transform.rotation.z + (clockwise ? 3 : -3));
                transform.rotation *= target;
            }
            else if (3 >= Mathf.Abs(transform.rotation.eulerAngles.z - lastRotation.eulerAngles.z))
            {
                transform.rotation = lastRotation;
            }
            if (3 >= Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z))
            {
                transform.rotation = targetRotation;
                lastRotation = targetRotation;
            }
        }

        if (GetComponent<PolygonCollider2D>().IsTouchingLayers(LayerMask.GetMask(blockLayers[0],blockLayers[1])))
        {
            if (!blocked)
            {
                blocked = true;
                Debug.Log("It happening!!!");
                //Quaternion target = !clockwise ? rotationAmount : Quaternion.Inverse(rotationAmount);
                //transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation*target, Time.deltaTime * 4F);
                //transform.rotation = lastRotation;
                Quaternion target = Quaternion.Euler(0, 0, transform.rotation.z + (!clockwise ? 5 : -5));
                 transform.rotation *= target;
                //transform.rotation = lastRotation;
                timer = 0;
            }
            else
            {
                //Quaternion target = Quaternion.Euler(0, 0, transform.rotation.z + (!clockwise ? 1 : -1));
                //transform.rotation *= target;
            }
        }
        
        /*
        else
        {
            if (timer == 10)
            {
                lastRotation = transform.rotation;
                timer = 0;
            }
            else
                timer++;
        }*/
        
    }
}