using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotationCopier : MonoBehaviour {
    public Camera cam;
    private Quaternion offset;
	// Use this for initialization
	void Start () {
        offset = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = offset * cam.transform.rotation;
	}
}
