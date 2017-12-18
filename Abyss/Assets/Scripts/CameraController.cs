using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        Debug.Assert(rb);
        Input.gyro.enabled = true;
    }

    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GyroModifyCamera()
    {
        rb.AddTorque(GyroToUnity2D(Input.gyro.attitude));
    }

    private static float GyroToUnity2D(Quaternion q)
    {
        Quaternion toReturn =  new Quaternion(q.x, q.y, -q.z, -q.w);
        return toReturn.eulerAngles.z;
    }
}
