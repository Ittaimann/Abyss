﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //these are temp and used to have a full local list of objects in the scene
    public GameObject squareParent, squareOutlineParent, triangleParent, triangleOutlineParent;
    private List<Transform> squares, squareOutlines, triangles, triangleOutlines;
	// Use this for initialization
	void Start () {
        squares = new List<Transform>();
        foreach (Transform t in squareParent.transform)
            squares.Add(t);

        squareOutlines = new List<Transform>();
        foreach (Transform t in squareOutlineParent.transform)
            squareOutlines.Add(t);

        
        triangles = new List<Transform>();
        foreach (Transform t in triangleParent.transform)
            triangles.Add(t);

        triangleOutlines = new List<Transform>();
        foreach (Transform t in triangleOutlineParent.transform)
            triangleOutlines.Add(t);
    }
	
	// Update is called once per frame
	void Update () {
        manageShapes();
	}

    public void manageShapes()
    {
        //Debug.Log(squareOutlines.Count);
        if (triangleOutlines.Count == 0 && triangles.Count != 0)
        {
            Debug.Log("All triangle outlines eliminated, deleting triangles.");
            foreach (Transform t in triangles) {
                Destroy(t.gameObject);
            }
            triangles = new List<Transform>();
        }
            
        if (squareOutlines.Count == 0 && squares.Count != 0) {
            Debug.Log("All square outlines eliminated, deleting squares.");
            foreach (Transform t in squares)
            {
                Destroy(t.gameObject);
                squares = new List<Transform>();
            }
        }
            
    }

    public void removeSquareOutline(Transform toRemove)
    {
        squareOutlines.Remove(toRemove);
        Destroy(toRemove.gameObject);
    }

    public void removeTriangleOutline(Transform toRemove)
    {
        triangleOutlines.Remove(toRemove);
        Destroy(toRemove.gameObject);
    }
}
