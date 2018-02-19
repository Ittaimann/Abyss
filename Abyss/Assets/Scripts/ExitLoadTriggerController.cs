using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLoadTriggerController : MonoBehaviour {

    public GameObject player;
    public GameManager gm;

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
            gm.loadNextLevel();
    }
}
