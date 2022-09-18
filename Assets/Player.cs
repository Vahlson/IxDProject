using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator _animator;
    float velocity = 5.0f;
    public float acceleration = .2f;
    public float deceleration = 0.5f;
    int velocityHash;
    public Queue<Transform> waypoints = new Queue<Transform>();
    Transform targetWayPoint;
    private BoardManager _boardManager;


    void Start()
    {
        _boardManager = GameObject.FindObjectOfType<BoardManager>();
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");

    }

    void Update()
    {
        _animator.SetFloat(velocityHash, velocity);
        if (targetWayPoint == null && waypoints.Count > 0)
        {
            targetWayPoint = waypoints.Dequeue();
        }
        if (targetWayPoint != null)
        {
            moveToWaypoint();
        }
        velocity += Time.deltaTime * acceleration;


    }
    void moveToWaypoint()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);
        if (transform.position == targetWayPoint.position && waypoints.Count > 0)
        {
            _boardManager.RecyclePlatform(targetWayPoint.root.gameObject);
            targetWayPoint = waypoints.Dequeue();
        }

    }


}
