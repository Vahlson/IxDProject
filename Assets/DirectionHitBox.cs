
using UnityEngine;

public class DirectionHitBox : MonoBehaviour
{
    private BoardManager _boardManager;

    void Start()
    {
        _boardManager = GameObject.FindObjectOfType<BoardManager>();

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = this.transform.position;

            _boardManager.RecyclePlatform(this.transform.root.gameObject);
        }
    }
}
