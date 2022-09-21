using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public GameObject left;
    [SerializeField]
    public GameObject right;
    [SerializeField]
    public GameObject next;
    public bool isEnd = false;

    public void setNext(GameObject waypoint)
    {
        // isEnd = true;
        next = waypoint;

    }
    void Start()
    {

    }
    void Update()
    {
    }
}
