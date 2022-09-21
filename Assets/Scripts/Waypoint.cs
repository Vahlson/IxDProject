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
        next = waypoint;
    }
    void Start()
    {

    }
    void Update()
    {
    }
    public Transform getLeftMove()
    {
        return this.left == null ? null : this.left.transform;

    }
    public Transform getRightMove()
    {
        return this.right == null ? null : this.right.transform;
    }

}
