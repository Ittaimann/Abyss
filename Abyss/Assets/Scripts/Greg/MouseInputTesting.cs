using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputTesting : MonoBehaviour {

    public float playerSpeed;
    public float acceleration;
    public float doubleTapTime;
    public float movementIgnoreRadius;
    public float bounceForce;
    public float grabRange;
    public Camera cam;
    public GameManager gm;
    public bool isGhost = false;
    public GameObject ghostPlayer;

    private Quaternion offset;

    /// raycast stuff for bounce
    //RaycastHit hit;
    //Ray ray; 
    bool grounded;
    /// </summary>

    bool isDoubleTap;
    Coroutine doubleTapCoroutine;
    Vector2 desiredVelocity, previousDesiredVelocity, startingVelocity;
    Vector2 prevDirection, direction; // values are: -1 = left on screen, 0 = idle, 1 = right on screen
    float movementLerpStep;

    // TODO change this later, of course
    public PrototypeRotatorSystem rotatorSystem;

    TestPlayerAnimator animator;

    bool checkMovementIgnoreLast;

    void Start () {
        desiredVelocity = Vector2.zero;
        startingVelocity = Vector2.zero;
        offset = transform.rotation;
        animator = GetComponent<TestPlayerAnimator>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("collides");
        grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
   
    {

        if (grounded)
        {
            grounded = false;
        }
    }

    void Update () {
        //this block finds and interprets mouse click events in the scene
        if (!isGhost && Input.GetMouseButtonDown(0)) {
            if (isDoubleTap)
            {
                // If the double-tap coroutine is still running, stop it -- it can cause an issue at a specific edge case
                StopCoroutine(doubleTapCoroutine);
                isDoubleTap = false;

                //
                // ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log("Target Tag: " + hit.collider.gameObject.tag + "  Target Name: " + hit.collider.gameObject.name);

                    //BOUNCE

                    if(hit.collider.gameObject.tag == "Player")
                    {
                        this.GetComponent<Rigidbody2D>().AddForce(1.0f / 10 * bounceForce * -Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        if(ghostPlayer)ghostPlayer.GetComponent<Rigidbody2D>().AddForce(1.0f / 10 * bounceForce * Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        gm.bounceAllGroundedShapes(bounceForce);

                    //GRAB

                    }else if (hit.collider.gameObject.tag == "SolidObjects")
                    {
                        //could've actually hit the outer collider instead of the object itself, if so assign sc as that of the parent, the true object
                        ShapeController sc = (hit.collider.gameObject.name == "OuterCollider")?hit.collider.transform.parent.GetComponent<ShapeController>():hit.collider.gameObject.GetComponent<ShapeController>();
                        if (sc.isGrabbed())
                        {
                            sc.unbind();
                            Debug.Log("Player released \"" + hit.collider.transform.name + "\"");
                        }
                        // if clicked object is close enough to grab
                        else if (Vector3.Distance(transform.position, hit.collider.transform.position) <= grabRange)
                        {
                            sc.bind(transform);//this script is already attached to the player, so it is passing the transform of the player
                            Debug.Log("Player grabbed \"" + hit.collider.transform.name + "\"");
                        }
                        else
                        {
                            Debug.Log("Tried to bind object \"" + hit.collider.transform.name + "\" to player but player was too far away.");
                        }
                    }                    
                }
                else
                {
                  //JUMP

                    if (grounded)
                    {
                        //drop any objects grabbed then execute jump
                        gm.unbindAllPossibleObjects();
                        GetComponent<Rigidbody2D>().AddForce(1.0f / 10 * 150 * -Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        if(ghostPlayer)ghostPlayer.GetComponent<Rigidbody2D>().AddForce(1.0f / 10 * 150 * Physics2D.gravity.normalized, ForceMode2D.Impulse);
                        if (animator != null)
                            animator.SetState(PlayerState.JUMP, 16);
                    }
                }            
                
            } else {
                doubleTapCoroutine = StartCoroutine(DoubleTapCheck());
                isDoubleTap = true;
            }
        }

        bool checkMovementIgnore = CheckMovementIgnore();
        // There's a guard applied to ignore movement when the input is really close to the player's current position (prevents constantly moving)
        if (!checkMovementIgnore) {
            // Update direction values based on things like rotation direction, input position, and updating based on GetMouseButtonDown vs GetMouseButton
            RunDirectionHandler();
            
            // Apply velocity based on movement direction
            if (rotatorSystem.orientation == 0 || rotatorSystem.orientation == 2) {
                GetComponent<Rigidbody2D>().velocity = new Vector2(
                    Mathf.SmoothStep(playerSpeed * prevDirection.x, playerSpeed * direction.x, movementLerpStep),
                    GetComponent<Rigidbody2D>().velocity.y);
                if(ghostPlayer)ghostPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x, ghostPlayer.GetComponent<Rigidbody2D>().velocity.y);
            } else if (rotatorSystem.orientation == 1 || rotatorSystem.orientation == 3) {
                GetComponent<Rigidbody2D>().velocity = new Vector2(
                    GetComponent<Rigidbody2D>().velocity.x,
                    Mathf.SmoothStep(playerSpeed * prevDirection.y, playerSpeed * direction.y, movementLerpStep));
                if(ghostPlayer)ghostPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(ghostPlayer.GetComponent<Rigidbody2D>().velocity.x, -GetComponent<Rigidbody2D>().velocity.y);
            }
            

            if (movementLerpStep < 1f) {
                movementLerpStep += Time.deltaTime*acceleration;
            } else {
                movementLerpStep = 1f;
            }
        }
        if (checkMovementIgnore == false && checkMovementIgnoreLast == true) {
            if (Input.GetMouseButton(0)) {
                if (animator != null && animator.state != PlayerState.JUMP)
                    animator.SetState(PlayerState.WALK, 0);
            } else {
                if (animator != null && animator.state != PlayerState.JUMP)
                    animator.SetState(PlayerState.IDLE, 0);
            }
        } else if (checkMovementIgnore == true && checkMovementIgnoreLast == false) {
            if (animator != null && animator.state != PlayerState.JUMP)
                animator.SetState(PlayerState.IDLE, 0);
        }

        checkMovementIgnoreLast = checkMovementIgnore;
    }

    private void RunDirectionHandler() {
        bool updateStep = false;

        // GetMouseButtonDown is different from GetMouseButton because this is called without any prior movement to account for
        if (Input.GetMouseButtonDown(0)) {
            Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            direction = new Vector2(
                inputPos.x > transform.position.x ?  1 :
                inputPos.x < transform.position.x ? -1 : 0,
                inputPos.y > transform.position.y ?  1 :
                inputPos.y < transform.position.y ? -1 : 0);

            prevDirection = Vector2.zero;
            updateStep = true;
            if (animator != null && animator.state != PlayerState.JUMP)
                animator.SetState(PlayerState.WALK, 0);
        }

        // GetMouseButton is different since we're switching direction, instead of starting to move without a prior direction to remember
        if (Input.GetMouseButton(0)) {
            Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // This looks complicated -- it's just ~50 lines of nested if-statements condensed down into some ternary operations

            // We could be getting GetMouseButton without having done GetMouseButtonDown first -- it's copy/paste but it's minor
            if (direction.x == 0 || direction.y == 0) {
                direction = new Vector2(
                    inputPos.x > transform.position.x ? 1 :
                    inputPos.x < transform.position.x ? -1 : 0,
                    inputPos.y > transform.position.y ? 1 :
                    inputPos.y < transform.position.y ? -1 : 0);
                prevDirection = Vector2.zero;
                updateStep = true;
            } else {
                Vector2 lastDirection = direction;
                direction = new Vector2(
                    inputPos.x > transform.position.x && direction.x == -1 ? 1 :
                    inputPos.x < transform.position.x && direction.x ==  1 ? -1 :
                    direction.x,
                    inputPos.y > transform.position.y && direction.y == -1 ? 1 :
                    inputPos.y < transform.position.y && direction.y ==  1 ? -1 :
                    direction.y);
                if ((rotatorSystem.orientation == 0 || rotatorSystem.orientation == 2) && lastDirection.x != direction.x ||
                    (rotatorSystem.orientation == 1 || rotatorSystem.orientation == 3) && lastDirection.y != direction.y)
                    updateStep = true;
            }

            // Set sprite based on mouse input
            if (rotatorSystem.orientation == 0) {
                animator.facingRight = inputPos.x > transform.position.x;
            } else if (rotatorSystem.orientation == 2) {
                animator.facingRight = inputPos.x < transform.position.x;
            } else if (rotatorSystem.orientation == 1) {
                animator.facingRight = inputPos.y < transform.position.y;
            } else if (rotatorSystem.orientation == 3) {
                animator.facingRight = inputPos.y > transform.position.y;
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            prevDirection = direction;
            direction = Vector2.zero;
            updateStep = true;

            if (animator != null && animator.state != PlayerState.JUMP)
                animator.SetState(PlayerState.IDLE, 0);
        }

        // Updates the Lerp interval when a change in direction has been initiated.

        // For example: say our movement lerp looked like Lerp(0, speed, 0.8) where speed = 5 (Lerp would return 4)
        // and then we wanted to go the other way smoothly. Our new lerp would have to be (speed, -speed, X) where X would return
        // the same value for Lerp as it did previously (4)... so we calculate that here.
        if (updateStep) {
            updateStep = false;
            if (rotatorSystem.orientation == 0 || rotatorSystem.orientation == 2) {
                // set movementLerpStep to 0 when a divide by zero error would occur
                if (startingVelocity.x - desiredVelocity.x == 0)
                    movementLerpStep = 0;
                else
                    movementLerpStep = Mathf.Abs((startingVelocity.x - GetComponent<Rigidbody2D>().velocity.x) / (startingVelocity.x - desiredVelocity.x));
            } else if (rotatorSystem.orientation == 1 || rotatorSystem.orientation == 3) {
                if (startingVelocity.y - desiredVelocity.y == 0)
                    movementLerpStep = 0;
                else
                    movementLerpStep = Mathf.Abs((startingVelocity.y - GetComponent<Rigidbody2D>().velocity.y) / (startingVelocity.y - desiredVelocity.y));
            }
        }
    }

    private IEnumerator DoubleTapCheck() {
        yield return new WaitForSeconds(doubleTapTime);
        isDoubleTap = false;
    }

    private bool CheckMovementIgnore() {
        bool ignore = false;

        if (Input.GetMouseButton(0)) {
            Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (rotatorSystem.orientation == 0 || rotatorSystem.orientation == 2) {
                if (inputPos.x > transform.position.x - movementIgnoreRadius && inputPos.x < transform.position.x + movementIgnoreRadius)
                    ignore = true;
            } else if (rotatorSystem.orientation == 1 || rotatorSystem.orientation == 3) {
                if (inputPos.y > transform.position.y - movementIgnoreRadius && inputPos.y < transform.position.y + movementIgnoreRadius)
                    ignore = true;
            }
        }

        return ignore;
    }
}