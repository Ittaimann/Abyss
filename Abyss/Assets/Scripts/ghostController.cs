using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostController : MonoBehaviour {
    public Transform player;// to read position and rotation values
    public Transform playerRoom;
    private Vector3 mirrorPoint;//position from the playerRoom Transform
    public Transform ghostRoom;
    private Vector3 offset;//difference between positions in playerRoom and ghostRoom
    public TestPlayerAnimator playerAnimator;
    private TestPlayerAnimator animator;// copies the state of the player Animator
    void Start () {
        mirrorPoint = playerRoom.position;
        offset = ghostRoom.position - playerRoom.position;
        animator = GetComponent<TestPlayerAnimator>();
    }
	
	void Update () {
        //sets ghost position to player position across from mirror point
        transform.position = 2 * mirrorPoint - player.transform.position + offset;
        //set ghost rotation to opposite of player rotation
        transform.rotation = Quaternion.Euler(0, 0, player.transform.rotation.eulerAngles.z + 180);
        //copy animations
        animator.SetState(playerAnimator.state, playerAnimator.currentFrame);
	}
}