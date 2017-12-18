using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour {
	private Rigidbody2D rb2D;
	public float offset;

    // Use this for initialization
    void Start () {
        rb2D = GetComponent<Rigidbody2D>();
		offset=Input.gyro.attitude.eulerAngles.z;
    }
	
	// Update is called once per frame
	void Update () {
		print(Input.gyro.attitude.eulerAngles.z);
        transform.localEulerAngles=new Vector3(0,0,Input.gyro.attitude.eulerAngles.z-offset);


    }

	
}
