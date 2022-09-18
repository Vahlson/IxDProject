using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // private BoardManager _boardManager;
    private void OnEnable()
    {
        // _boardManager = GameObject.FindObjectOfType<BoardManager>();
    }
    // private void OnBecameInvisible(){
    //     _boardManager.RecyclePlatform(this.gameObject);

    // }
    void OnTriggerEnter(Collider other)
    {
        // if (other.tag == "Player")
        // {
        //     _boardManager.RecyclePlatform(this.gameObject);
        // }
    }
}
