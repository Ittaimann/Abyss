using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour {
    private bool grounded = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter2D()
    {

    }
    public bool isGrounded()
    {
        return grounded;
    }
}
