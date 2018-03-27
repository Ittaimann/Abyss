using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState { IDLE, WALK, JUMP };

public class TestPlayerAnimator : MonoBehaviour {
    public string walkFolder;
    public string jumpFolder;
    public int fps;

    Sprite[] walkSprites;
    Sprite[] jumpSprites;

    SpriteRenderer sprRender;
    Rigidbody2D rb2D;

    public int currentFrame;
    public PlayerState state;


    public bool facingRight = true;

    void Start () {
        walkSprites = Resources.LoadAll<Sprite>(walkFolder);
        jumpSprites = Resources.LoadAll<Sprite>(jumpFolder);
        sprRender = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(AnimationCoroutine());
    }
    
    // Using a coroutine because we want framerate-based animation, it's less intuitive measuring time in Update() or smt
    private IEnumerator AnimationCoroutine() {
        while (true) {
            if (currentFrame < 0) {
                currentFrame = 0;
            }
            switch (state) {
                case PlayerState.IDLE:
                    sprRender.sprite = jumpSprites[0];
                    break;
                case PlayerState.WALK:
                    if (currentFrame >= walkSprites.Length)
                        currentFrame = 0;
                    sprRender.sprite = walkSprites[currentFrame++];
                    break;
                case PlayerState.JUMP:
                    if (currentFrame >= jumpSprites.Length) {
                        currentFrame = 0;
                        if (Input.GetMouseButton(0)) {
                            state = PlayerState.WALK;
                        } else {
                            state = PlayerState.IDLE;
                        }
                    }
                    sprRender.sprite = jumpSprites[currentFrame++];
                    break;
            }
            sprRender.flipX = !facingRight;
            yield return new WaitForSeconds(1f/fps);
        }
    }

    public void SetState(PlayerState playerState, int frame) {
        state = playerState;
        currentFrame = frame;

        if (currentFrame < 0) {
            currentFrame = 0;
        }
        switch (state) {
            case PlayerState.IDLE:
                sprRender.sprite = jumpSprites[0];
                break;
            case PlayerState.WALK:
                if (currentFrame >= walkSprites.Length)
                    currentFrame = 0;
                sprRender.sprite = walkSprites[currentFrame];
                break;
            case PlayerState.JUMP:
                if (currentFrame >= jumpSprites.Length)
                    currentFrame = 0;
                sprRender.sprite = jumpSprites[currentFrame];
                break;
        }
    }

    public void ResetState() {
        currentFrame = 0;
        switch (state) {
            case PlayerState.IDLE:
                sprRender.sprite = jumpSprites[0];
                break;
            case PlayerState.WALK:
                sprRender.sprite = walkSprites[0];
                break;
            case PlayerState.JUMP:
                sprRender.sprite = jumpSprites[0];
                break;
        }
    }
}
