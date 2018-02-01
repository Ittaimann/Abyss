﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputTesting : MonoBehaviour {

    public float playerSpeed;
    public float acceleration;

    Vector2 desiredVelocity, startingVelocity;
    float movementLerpStep;
    Vector3 previousMousePos;

    // TODO change this later, of course
    PrototypeRotatorSystem rotatorSystem;

    void Start () {
        desiredVelocity = Vector2.zero;
        startingVelocity = Vector2.zero;
    }
    
    void Update () {

        if (Input.GetMouseButton(0)) {
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Mathf.Sign(currentMousePos.x) != Mathf.Sign(previousMousePos.x)) {
                movementLerpStep = 1 - (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) / playerSpeed);
                startingVelocity = desiredVelocity;
                desiredVelocity = new Vector2(playerSpeed * Mathf.Sign(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x), desiredVelocity.y);
            }
            previousMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(0)) {
            movementLerpStep = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) / playerSpeed;
            startingVelocity = Vector2.zero;
            desiredVelocity = new Vector2(playerSpeed * Mathf.Sign(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x), desiredVelocity.y);
        }

        if (Input.GetMouseButtonUp(0)) {
            movementLerpStep = 1 - (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) / playerSpeed);
            startingVelocity = desiredVelocity;
            desiredVelocity = Vector2.zero;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(
            Mathf.SmoothStep(startingVelocity.x, desiredVelocity.x, movementLerpStep),
            GetComponent<Rigidbody2D>().velocity.y);

        if (movementLerpStep < 1f) {
            movementLerpStep += Time.deltaTime*acceleration;
        } else {
            movementLerpStep = 1f;
        }

    }
}
