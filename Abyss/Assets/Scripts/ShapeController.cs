using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour {

    private bool isPositionallyBound = false;
    private Transform toCopy;
    private Vector3 initOffset = new Vector3(0f,2f,0f);
    //private Transform oldParent;

    // Update is called once per frame
    void Update()
    {
        transform.position = isPositionallyBound ? toCopy.position + initOffset : transform.position;
    }

    public void bind(Transform other)
    {
        //oldParent = transform.parent;
        //transform.parent = other;
        initOffset =  transform.position - other.transform.position + (Vector3)(-0.1f * Physics2D.gravity.normalized);
        isPositionallyBound = true;
        toCopy = other;
        this.gameObject.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = true;
        //other.GetComponent<Rigidbody2D>().isKinematic = true;
        this.gameObject.transform.GetComponent<Rigidbody2D>().isKinematic = true;

    }

    public void unbind()
    {
        //transform.parent = oldParent;
        //initOffset = new Vector3();
        isPositionallyBound = false;
        this.gameObject.transform.GetChild(0).GetComponent<Collider2D>().isTrigger = false;
        this.gameObject.transform.GetComponent<Rigidbody2D>().isKinematic = false;
        toCopy = null;
    }
    public bool isGrabbed()
    {
        return isPositionallyBound;
    }

    //when an object is bound their collider gets set to trigger instead of normal, so it can not mess with things
    //but so it can also not leave the playspace
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player" && other.gameObject.layer + 1 != this.gameObject.layer && other.gameObject.layer != this.gameObject.layer)
        {
            Debug.Log(this.gameObject.tag + " on layer #" + this.gameObject.layer + " Collided with: " + other.tag + " on layer #" + other.gameObject.layer);
            //other.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            
            unbind();
        }
    }
}
