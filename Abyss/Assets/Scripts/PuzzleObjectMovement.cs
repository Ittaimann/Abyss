using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObjectMovement : MonoBehaviour {
	public float speed;
	private Rigidbody2D rb;
	private Camera cam;
	private Collider2D coll;
	private SpriteRenderer spriteRenderer;
	private Texture2D normal_texture;
	private Texture2D touched_texture;
	private Sprite normal_sprite;
	private Sprite touched_sprite;
	private bool touched = false;
	
	void Awake() {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
		coll = GetComponent<Collider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		Debug.Assert(coll);
		Debug.Assert(cam);
		Debug.Assert(rb);
	}

	// Use this for initialization
	void Start () {
		normal_texture = Resources.Load("Square") as Texture2D;
		touched_texture = Resources.Load("TouchedSquare") as Texture2D;
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		normal_sprite = Sprite.Create(normal_texture, new Rect(0,0, normal_texture.width, normal_texture.height), pivot);
		touched_sprite = Sprite.Create(touched_texture, new Rect(0,0, touched_texture.width, touched_texture.height), pivot);
		Debug.Assert(normal_sprite);
		Debug.Assert(touched_sprite);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);

			RaycastHit2D hit = 
				Physics2D.Raycast(cam.ScreenToWorldPoint(touch.position), Vector2.zero);

			if(touch.phase != TouchPhase.Ended &&
			   hit && hit.transform.gameObject == gameObject) {
				if(!touched) {
					touched = true;
					spriteRenderer.sprite = touched_sprite;
				}
				switch(Input.touchCount) {
					case 1:
						RotateWithTouch(touch);
						break;
					case 2:
						MoveWithTouch(touch);
						break;
				}
			} else if(touched) {
				touched = false;
				spriteRenderer.sprite = normal_sprite;
			}
		}
	}

	Vector2 startTouchPos = Vector2.zero;
	Vector2 radius_start = Vector2.zero;
	void RotateWithTouch(Touch touch) {
		Vector2 center = coll.bounds.center;

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

	void MoveWithTouch(Touch touch) {
		if(touch.phase == TouchPhase.Moved) {
			Vector2 endTouchPos = cam.ScreenToWorldPoint(touch.position);
			transform.position = endTouchPos;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		rb.AddForce(-2 * rb.velocity);
	}
}
