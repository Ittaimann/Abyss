using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectMovement : MonoBehaviour {
	public float speed;
	private Rigidbody2D rb;
	private Camera cam;
	private Collider2D coll;
	
	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
		coll = GetComponent<Collider2D>();
		Debug.Assert(coll);
		Debug.Assert(cam);
		Debug.Assert(rb);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch(Input.touchCount) {
			case 1:
				RotateWithTouch();
				break;
			case 2:
				MoveWithTouch();
				break;
		}
	}

	Vector2 startTouchPos = Vector2.zero;
	Vector2 radius_start = Vector2.zero;
	void RotateWithTouch() {
		Vector2 center = coll.bounds.center;
		Touch touch = Input.GetTouch(0);

		switch(touch.phase) {
			case TouchPhase.Began:
				startTouchPos = cam.ScreenToWorldPoint(touch.position);
				radius_start = startTouchPos - center;
				break;
			case TouchPhase.Moved:
				Vector2 endTouchPos = cam.ScreenToWorldPoint(touch.position);
				Vector2 radius_end = endTouchPos - center;
				float angle = toDegrees(angleBetween2D(radius_start, radius_end));
				transform.Rotate(Vector3.forward * angle);
				startTouchPos = endTouchPos;
				radius_start = startTouchPos - center;
				break;
		}
	}

	float angleBetween2D(Vector2 start, Vector2 end) {
		float sign = Vector3.Cross(start, end).z;
		return (sign >= 0 ? 1 : -1) * Mathf.Acos(Vector2.Dot(start, end) / (start.magnitude * end.magnitude));
	}

	float toDegrees(float f) {
		return f * (180 / Mathf.PI);
	}

	void MoveWithTouch() {
		Touch touch = Input.GetTouch(0);

		switch(touch.phase) {
			case TouchPhase.Moved:
				Vector2 endTouchPos = cam.ScreenToWorldPoint(touch.position);
				transform.position = endTouchPos;
				break;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		rb.AddForce(-2 * rb.velocity);
	}
}
