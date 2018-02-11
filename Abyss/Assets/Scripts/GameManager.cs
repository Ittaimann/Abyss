using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //these are temp and used to have a full local list of objects in the scene
    public GameObject squareParent, squareOutlineParent, triangleParent, triangleOutlineParent, circleParent, circleOutlineParent;
    private List<Transform> squares, squareOutlines, triangles, triangleOutlines, circles, circleOutlines;
    
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

        circles = new List<Transform>();
        foreach (Transform t in circleParent.transform)
            circles.Add(t);

        circleOutlines = new List<Transform>();
        foreach (Transform t in circleOutlineParent.transform)
            circleOutlines.Add(t);
    }
	
	// Update is called once per frame
	void Update () {
        manageShapes();
	}
    public void bounceAllGroundedShapes(float bounceStrength)
    {
        ContactPoint2D[] contPoints = new ContactPoint2D[16];
        Collider2D[] collisions = new Collider2D[16];
        foreach (Transform square in squares)
        {
            square.GetComponent<Rigidbody2D>().GetContacts(collisions);
            foreach (Collider2D coll in collisions)
                if (coll && coll.tag == "Room")
                {
                    coll.GetContacts(contPoints);
                    foreach (ContactPoint2D cp in contPoints)
                        if (cp.normal.normalized == Physics2D.gravity.normalized)
                        {
                            square.GetComponent<Rigidbody2D>().velocity = new Vector2();
                            square.GetComponent<Rigidbody2D>().AddForce(-bounceStrength * Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        }
                }
        }
        foreach (Transform triangle in triangles)
        {
            triangle.GetComponent<Rigidbody2D>().GetContacts(collisions);
            foreach (Collider2D coll in collisions)
                if (coll && coll.tag == "Room")
                {
                    coll.GetContacts(contPoints);
                    foreach (ContactPoint2D cp in contPoints)
                        if (cp.normal.normalized == Physics2D.gravity.normalized)
                        {
                            triangle.GetComponent<Rigidbody2D>().velocity = new Vector2();
                            triangle.GetComponent<Rigidbody2D>().AddForce(-bounceStrength * Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        }
                            
                }
        }
        foreach (Transform circle in circles)
        {
            circle.GetComponent<Rigidbody2D>().GetContacts(collisions);
            foreach (Collider2D coll in collisions)
                if (coll && coll.tag == "Room")
                {
                    coll.GetContacts(contPoints);
                    foreach (ContactPoint2D cp in contPoints)
                        if (cp.normal.normalized == Physics2D.gravity.normalized)
                        {
                            circle.GetComponent<Rigidbody2D>().velocity = new Vector2();
                            circle.GetComponent<Rigidbody2D>().AddForce(-bounceStrength * Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        }
                }
        }
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

        if (squareOutlines.Count == 0 && squares.Count != 0)
        {
            Debug.Log("All square outlines eliminated, deleting squares.");
            foreach (Transform t in squares)
            {
                Destroy(t.gameObject);
                squares = new List<Transform>();
            }
        }

        if (circleOutlines.Count == 0 && circles.Count != 0)
        {
            Debug.Log("All circle outlines eliminated, deleting circles.");
            foreach (Transform t in circles)
            {
                Destroy(t.gameObject);
                circles = new List<Transform>();
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

    public void removeCircleOutline(Transform toRemove)
    {
       circleOutlines.Remove(toRemove);
        Destroy(toRemove.gameObject);
    }
}
