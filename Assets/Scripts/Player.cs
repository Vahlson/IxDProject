using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private Animator _animator;
    public float velocity = 5.0f;
    public float rotationSpeed = 2.0f;
    public float acceleration = .2f;
    int velocityHash;
    public Transform targetWayPoint;
    [SerializeField] private int maxHealth = 10;
    public int currentHealth;

    //Lane Switching
    private float targetLaneTargetOffset = 0;
    private float targetLaneOffset = 0;
    [SerializeField] private float laneSwitchTime = 1f;
    private float laneSwitchTimeElapse;

    private Vector3 moveToPosition;

    [HideInInspector] public float score;

    void Awake()
    {
        currentHealth = maxHealth;

    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");

        //Start by not lerping position
        laneSwitchTimeElapse = laneSwitchTime;
    }

    void Update()
    {
        _animator.SetFloat(velocityHash, velocity);
        if (targetWayPoint != null)
        {
            moveToWaypoint();
        }
        if (Input.GetKeyUp("d"))
        {
            moveRight();
        }
        if (Input.GetKeyUp("a"))
        {
            moveLeft();
        }

        score += Time.deltaTime;

        //print(laneSwitchTimeElapse);
        /* if (laneSwitchTimeElapse < laneSwitchTime && laneSwitchTime != 0)
        {
            targetLaneOffset = Mathf.Lerp(0, targetLaneTargetOffset, laneSwitchTimeElapse / laneSwitchTime);
            laneSwitchTimeElapse += Time.deltaTime;
            //print("HEJJJJJJJJJJJJ");

            print("Offset: " + targetLaneOffset);
            //transform.position += transform.right * targetLaneOffset;
        } */



        velocity += Time.deltaTime * acceleration;
    }
    private void moveToWaypoint()
    {

        //Getting the offset of the component that is forward between waypoint and player. so if z is of forward, what is their z offset.
        float playerToTargetForwardOffset = Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) - Vector3.Dot(targetWayPoint.forward, transform.position);
        moveToPosition = targetWayPoint.position + targetWayPoint.forward * (2.5f - playerToTargetForwardOffset);
        print("movetoPos: " + moveToPosition);
        print("targetWaypoinyPos: " + targetWayPoint.position);

        //The old, working version.
        //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        //transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);

        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, moveToPosition, velocity * Time.deltaTime);

    }

    private void startLerpBetweenLanes(float targetOffset)
    {
        targetLaneTargetOffset = targetOffset;
        targetLaneOffset = 0;
    }

    private void moveLeft()
    {
        Transform move = targetWayPoint.GetComponent<Waypoint>().getLeftMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * -5;
            startLerpBetweenLanes(-5);

        }
    }
    private void moveRight()
    {
        Transform move = targetWayPoint.GetComponent<Waypoint>().getRightMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * 5;
            startLerpBetweenLanes(5);
        }
    }
    public bool hasReachedTarget()
    {
        //return transform.position == targetWayPoint.transform.position;
        //return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));

        return Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) >= 0;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            takeDamage();
            //print("Hit obstacle");
        }
    }

    public void takeDamage()
    {
        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            PlayerPrefs.SetInt("Score", (int)score);
            SceneManager.LoadScene(2);
        }
    }
}
