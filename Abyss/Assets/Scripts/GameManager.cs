using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject exitParent;//deactivated upon start, reactivated upon all object lists being emptied.
    //these are temp and used to have a full local list of objects in the scene
    public GameObject squareParent, squareOutlineParent, triangleParent, triangleOutlineParent, circleParent, circleOutlineParent;
    private List<Transform> squares, squareOutlines, triangles, triangleOutlines, circles, circleOutlines;

    public string[] bufferedScenes;//a list of all scenes that can need to be loaded
    
	// Use this for initialization
	void Start () {
        exitParent.SetActive(false);

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

        foreach (string s in bufferedScenes)
            StartCoroutine(LoadScene(s));
    }
	
	// Update is called once per frame
	void Update () {
        manageShapes();

        if (isLevelClear())
            activateExits();
        //temp testing code
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
                Debug.Log(SceneManager.GetSceneAt(i));
            loadScene("Assets/Scenes/Puzzle3.unity");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            loadScene("Assets/Scenes/Puzzle2.unity");
        }
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

    public bool isLevelClear()
    {
        return (circleOutlines.Count == 0 && squareOutlines.Count == 0 && triangleOutlines.Count == 0);
    }

    public void activateExits()
    {
        exitParent.SetActive(true);
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

    public void loadScene(string sceneName)
    {
        Scene toUnload = SceneManager.GetActiveScene();
        //if the scene you are trying to set as active is ready to be loaded, set it, then start unloading the old scene.
        if (SceneManager.SetActiveScene(SceneManager.GetSceneByPath(sceneName)))
            StartCoroutine(UnloadScene(toUnload));
    }

    private IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            Debug.Log("Finished loading scene: \"" + sceneToLoad + '\"');
            
            /*foreach (GameObject g in FindObjectsOfType<GameObject>())
            {
               // Debug.Log(g.scene.name + " vs " + SceneManager.GetActiveScene().name + "\n" + SceneManager.GetSceneByPath(sceneToLoad).name);
                if (g.scene != SceneManager.GetActiveScene())
                {
                    g.SetActive(false);
                }
            }*/
            yield return null;
        }
    }

    private IEnumerator UnloadScene(Scene sceneToUnload)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }

}
