using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject exitParent;//deactivated upon start, reactivated upon all object lists being emptied.
    //these are temp and used to have a full local list of objects in the scene
    public GameObject squareParent, squareOutlineParent, triangleParent, triangleOutlineParent, circleParent, circleOutlineParent;
    private List<Transform> squares, squareOutlines, triangles, triangleOutlines, circles, circleOutlines;

    public string next, restart;//scenes paths to preload
    
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

        if (this.gameObject.scene == SceneManager.GetActiveScene())
            preloadSceneReferences();
        
    }
	
    public void preloadSceneReferences()// needs to be public in order for destroyed scene's gameManager to trigger the next gameManager to load their references
    {
        //if the path to a scene is not provided, there is no scene therefore do nothing
        if (next != "")
        {
            // if there was a scene path, check to see if it was already loaded in, if not load it in.
            Scene test = SceneManager.GetSceneByPath(next);
            if (!test.IsValid())
            {
                StartCoroutine(LoadScene(next));
            }
        }

        if (restart != "")
        {
            Scene test = SceneManager.GetSceneByPath(restart);
            if (!test.IsValid())
            {
                StartCoroutine(LoadScene(restart));
            }
        }
    }
	// Update is called once per frame
	void Update () {
        manageShapes();

        if (isLevelClear())
            activateExits();
        if (Input.GetKeyDown(KeyCode.R) && restart != "")
        {
            //restart level
            loadScene(restart);
        }
    }
    public void loadNextLevel()
    {
        loadScene(next);
    }
    public void bounceAllGroundedShapes(float bounceStrength)
    {
        //bounce any held objects, as they are not touching the ground and therefore will not be bounced by the following code
        bounceAllBoundShapes(bounceStrength);
        //then drop any held objects
        unbindAllPossibleObjects();
        //then do the actual checking
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
    private void bounceAllBoundShapes(float bounceStrength)
    {
        foreach (Transform square in squares)
        {
            if (square.GetComponent<ShapeController>().isGrabbed())
            {
                square.GetComponent<ShapeController>().unbind();
                square.GetComponent<Rigidbody2D>().velocity = new Vector2();
                square.GetComponent<Rigidbody2D>().AddForce(-bounceStrength * Physics2D.gravity.normalized, ForceMode2D.Impulse);
            }
        }

        foreach (Transform triangle in triangles)
        {
            if (triangle.GetComponent<ShapeController>().isGrabbed())
            {
                triangle.GetComponent<ShapeController>().unbind();
                triangle.GetComponent<Rigidbody2D>().velocity = new Vector2();
                triangle.GetComponent<Rigidbody2D>().AddForce(-bounceStrength * Physics2D.gravity.normalized, ForceMode2D.Impulse);
            }
        }

        foreach (Transform circle in circles)
        {
            if (circle.GetComponent<ShapeController>().isGrabbed())
            {
                circle.GetComponent<ShapeController>().unbind();
                circle.GetComponent<Rigidbody2D>().velocity = new Vector2();
                circle.GetComponent<Rigidbody2D>().AddForce(-bounceStrength * Physics2D.gravity.normalized, ForceMode2D.Impulse);
            }
        }
    }
    private void manageShapes()
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

    public void unbindAllPossibleObjects()
    {
        foreach (Transform square in squares)
        {
            if (square.GetComponent<ShapeController>().isGrabbed())
            {
                square.GetComponent<ShapeController>().unbind();
            }
        }

        foreach(Transform triangle in triangles)
        {
            if (triangle.GetComponent<ShapeController>().isGrabbed())
            {
                triangle.GetComponent<ShapeController>().unbind();
            }
        }

        foreach (Transform circle in circles)
        {
            if (circle.GetComponent<ShapeController>().isGrabbed())
            {
                circle.GetComponent<ShapeController>().unbind();
            }
        }
    }
    public bool isLevelClear()
    {
        return (circleOutlines.Count == 0 && squareOutlines.Count == 0 && triangleOutlines.Count == 0);
    }

    private void activateExits()
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

    //given a string path to a scene that has *already* been loaded into the game, it will swap focus to that scene and delete the current and invoke the generation of any referenced scenes
    // this function will also clean up any preloaded scenes that are no longer referenced by the active scene
    private void loadScene(string sceneName)
    {
        Scene toUnload = SceneManager.GetActiveScene();
        //if the scene you are trying to set as active is ready to be loaded, set it, make everything in it active (except exits), and finally start unloading the old scene.
        if(SceneManager.GetSceneByPath(sceneName).isLoaded && SceneManager.SetActiveScene(SceneManager.GetSceneByPath(sceneName)))
        {
            //deactivate all objects in deleting scene (so they don't bump things on their way out)
            foreach(GameObject g in toUnload.GetRootGameObjects())
            {
                if(g.name != "GameManager")//GameManager must be preserved in order to actually call the UnloadScene Coroutine
                    g.SetActive(false);
            }
            //activate all objects in loading scene
            foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (g.scene == SceneManager.GetActiveScene() && g.name != "Exits")
                {
                    g.SetActive(true);
                    if(g.name == "GameManager")
                    {
                        //normally this is called in the start function of a gameobject, but because of the way scenes are being loaded in (spawn all objects first then deactivate and wait)
                        //this needs to be called on the new scenes game manager when it is being made active so that way it knows when to load its referenced scenes.
                        g.GetComponent<GameManager>().preloadSceneReferences();
                    }
                }
            }
            StartCoroutine(UnloadScene(toUnload));
            //now that the scene swapped from the current to a new one, unload any scenes that were preloaded but are no longer called for in the references of the current scene.
            //Debug.Log("After switching the remaining loaded scenes are as follows: ");
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                Scene temp = SceneManager.GetSceneAt(i);
                //Debug.Log(temp.name + " which has an isLoaded value of " + temp.isLoaded);
                //if a scene is still loaded but it isnt the active scene, it was a preloaded scene left over from the last scene and therefore could need to be removed
                if(temp.isLoaded && temp != SceneManager.GetActiveScene())
                {
                    bool shouldUnload = false;
                    foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects()) {
                        //if(g.name == "GameManager")
                        //  Debug.Log(temp.path + " " + g.GetComponent<GameManager>().restart + " " + g.GetComponent<GameManager>().next);
                        if (g.name == "GameManager" && !(g.GetComponent<GameManager>().restart == temp.path || g.GetComponent<GameManager>().next == temp.path)) {
                            shouldUnload = true;
                            break;
                        }
                    }
                    if(shouldUnload)
                        StartCoroutine(UnloadScene(temp));
                }
            }
        }
        else
        {
            Debug.Log("Failed to switch to a scene. Wasn't yet loaded.");
        }
    }

    private IEnumerator LoadScene(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //uncomment to exaggerate issue
        //for(int i = 0; i < 10; ++i)
        //   yield return null;
        foreach (GameObject g in SceneManager.GetSceneByPath(sceneToLoad).GetRootGameObjects())
        {
            if (g.scene != SceneManager.GetActiveScene())
            {
                g.SetActive(false);
            }
        }
        Debug.Log("Finished loading scene: \"" + sceneToLoad + '\"' + "\nFull Path: " + SceneManager.GetSceneByPath(sceneToLoad).path);
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
