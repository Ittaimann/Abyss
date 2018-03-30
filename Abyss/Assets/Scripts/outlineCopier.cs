using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outlineCopier : MonoBehaviour {
    public bool isInitiallyFrozen = false;
    public Transform outlineToCopy;
    public bool copyFreezeState = false;
    //the GameManager automatically uses the previous bools to identify and manage frozen states
    public bool copyRotation = false;
    	
	void Update () {
        if (copyRotation)
            transform.rotation = outlineToCopy.transform.rotation;
	}
}