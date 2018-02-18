using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeRotatorSystem : MonoBehaviour {

    public int orientation;
    public float rotationSpeed;
    public Rigidbody2D player;
    private Dictionary<DeviceOrientation, int> orientationMap;

    Coroutine rotationCoroutine;
    Quaternion previousRot;

    void Start() {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        orientationMap = new Dictionary<DeviceOrientation, int>();
        orientationMap[DeviceOrientation.LandscapeLeft] = 0;
        orientationMap[DeviceOrientation.PortraitUpsideDown] = 1;
        orientationMap[DeviceOrientation.LandscapeRight] = 2;
        orientationMap[DeviceOrientation.Portrait] = 3;
        previousRot = player.transform.localRotation;
    }

    void Awake() {
         Input.gyro.enabled = true;
    }

    void Update () {

        int prevOrientation = orientation;

        // Ittai I swear to jesus on crackers if you lose your marbles because this prototype system uses GetKeyDown()...
        if (Input.GetKeyDown(KeyCode.P) || (orientationMap.ContainsKey(Input.deviceOrientation) && orientationMap[Input.deviceOrientation] < orientation)) {
            orientation = --orientation < 0 ? 3 : orientation;
            //Debug.Log(orientation);
        }
        if (Input.GetKeyDown(KeyCode.O) || (orientationMap.ContainsKey(Input.deviceOrientation) && orientationMap[Input.deviceOrientation] > orientation)) {
            orientation = ++orientation > 3 ? 0 : orientation;
        }

        // if (orientationMap.ContainsKey(Input.deviceOrientation) && orientationMap[Input.deviceOrientation] != orientation)
        // {
            // orientation = orientationMap[Input.deviceOrientation];
        // }

        // Rotate camera based on the orientation value

        // Starting here (arrow represents bottom of phone):

        //    ----------------------
        //   |  ------------------  |
        //   | |                  | |
        //   |O|      <----       | |  = 0
        //   | |                  | |
        //   |  ------------------  |
        //    ----------------------

        // Rotate counter-clockwise 90deg (vertical phone)    = 1
        // Rotate another 90deg (above model but upside down) = 2
        // Rotate another 90deg (vertical phone, upside down) = 3

        // Stop a rotation coroutine if one is currently running
        if (orientation != prevOrientation && rotationCoroutine != null) {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        if (orientation == 0 && prevOrientation == 3) {
            // Special case: counter-clockwise to 0 from 3
            rotationCoroutine = StartCoroutine(RotationCoroutine(-90, true));
        } else if (orientation == 3 && prevOrientation == 0) {
            // Special case: clockwise to 3 from 0
            rotationCoroutine = StartCoroutine(RotationCoroutine(90, false));
        } else {
            if (orientation > prevOrientation) {
                // Counter-clockwise
                rotationCoroutine = StartCoroutine(RotationCoroutine(-90, false));
            } else if (orientation < prevOrientation) {
                // Clockwise
                rotationCoroutine = StartCoroutine(RotationCoroutine(90, true));
            }
        }

        // SETTING GRAVITY IN HERE (we probably want this elsewhere, but this is a prototype, so who cares)
        switch (orientation) {
            case 0:
                Physics2D.gravity = new Vector2(0f, -9.81f);
                break;
            case 1:
                Physics2D.gravity = new Vector2(-9.81f, 0f);
                break;
            case 2:
                Physics2D.gravity = new Vector2(0f, 9.81f);
                break;
            case 3:
                Physics2D.gravity = new Vector2(9.81f, 0f);
                break;
        }
    }

    private IEnumerator RotationCoroutine(float changeZ, bool clockwise) {
        // Start where we currently are
        Quaternion start = player.transform.localRotation;

        // Using a reference "previousRot" instead of start to rotate on, to keep rotations locked at 90deg
        Quaternion end = previousRot * Quaternion.Euler(Vector3.forward * changeZ);
        previousRot = end;

        float step = 0f;

        // while condition basically says, "if we're close enough to our destination (1 degree), leave" -- it's fine to be close enough and just leave
        while ((player.transform.localRotation.eulerAngles.z-1) > end.eulerAngles.z || (player.transform.localRotation.eulerAngles.z+1) < end.eulerAngles.z) {
            player.transform.localRotation = Quaternion.Lerp(start, end, Mathf.Sin(step));
            yield return new WaitForSeconds(Time.deltaTime);
            step += Time.deltaTime*rotationSpeed;
            step = Mathf.Clamp(step, 0, Mathf.PI/2f);
        }

        // assuming we're not 100% on target from the while loop, lock the rotation to what we wanted to line it up perfectly
        player.transform.localRotation = end;

        // using a variable to check if rotation is already running -- we're done, so we set this to null
        rotationCoroutine = null;
    }
}
