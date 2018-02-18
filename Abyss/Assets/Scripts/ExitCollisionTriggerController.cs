using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCollisionTriggerController : MonoBehaviour {

    public GameObject room, player;
	
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
            Physics2D.IgnoreLayerCollision(13, 23, true);//Player and Room
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == player)
            Physics2D.IgnoreLayerCollision(13, 23, false);//Player and Room
    }
}
