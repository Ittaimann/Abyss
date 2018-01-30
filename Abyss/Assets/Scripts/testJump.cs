using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class testJump : MonoBehaviour {
    public float playerSpeed=400;
    
    private Rigidbody2D rb2d;

    
    float _threshold = 0.25f;

    void Start()
    {
   
        StartCoroutine("DetectTouchInput");
    }

    IEnumerator DetectTouchInput()
    {
        while (true)
        {
            float duration = 0;
            bool doubleClicked = false;
            if (Input.GetMouseButtonDown(0))
            {
                while (duration < _threshold)
                {
                    duration += Time.deltaTime;
                    yield return new WaitForSeconds(0.005f);
                    if (Input.GetMouseButtonDown(0))
                    {
                        doubleClicked = true;
                        duration = _threshold;
                        // Double click/tap
                        print("Double Click detected");

                        rb2d = GetComponent<Rigidbody2D>();
                             rb2d.AddForce(new Vector2(playerSpeed * Mathf.Sign(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), 350));






                    }
                }
                if (!doubleClicked)
                {
                    // Single click/tap
                    print("Single Click detected");
                    

                }
            }
            yield return null;
        }
    }
    private void Update()
    {
        
    }
}
