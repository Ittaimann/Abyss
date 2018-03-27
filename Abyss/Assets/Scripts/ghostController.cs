﻿using System.Collections;
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
    public GameManager gm;//for freezing and unfreezing outlines, also loading next level

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
        animator.facingRight = playerAnimator.facingRight;
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Outline")
        {
            gm.freezeOutline(other.transform.parent);//parent because the outer collider is what this will hit, but is not what is stored in gm
        }
    }
}