using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IttaiTesting2 : MonoBehaviour {

    private Rigidbody2D rb2D;
    public float offset;
	private Quaternion rot;
     public Vector3 smoother;
	 public float timer;


    // Use this for initialization
    void Start () {
		Input.gyro.enabled = true;
        rb2D = GetComponent<Rigidbody2D>();
        offset = Input.gyro.attitude.eulerAngles.z;
		smoother=Input.acceleration;
    }
	
	// Update is called once per frame
	void Update () {
		Vector3 lol=Vector3.zero;
		smoother=Vector3.SmoothDamp(smoother,Input.acceleration,ref lol, timer);
		float x=smoother.x;
        float y = smoother.y;
		float valraw=(Mathf.Atan2(x, y)*Mathf.Rad2Deg)-90;
        float val = valraw;
	
        if(valraw<-.00001)
		{
			val+=360;
		}
		print(val);
		if(Mathf.Abs(Input.acceleration.z)<.7f)
	    	transform.localEulerAngles = new Vector3(0, 0,(val+90));
    }
}
