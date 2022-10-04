using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public PlayerContainer playerContainer;
    private Animator _animator;
    public float velocity = 5.0f;
    public float acceleration = .2f;
    int velocityHash;
    public Transform targetWayPoint;
    [SerializeField] private int maxHealth = 10;
    public int currentHealth;
    [HideInInspector] public float score;
    public PlayerStance stance = PlayerStance.high;

    void Awake()
    {
        currentHealth = maxHealth;

    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
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


        velocity += Time.deltaTime * acceleration;
    }
    private void moveToWaypoint()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);
    }
    private void moveLeft()
    {
        Transform move = targetWayPoint.GetComponent<Waypoint>().getLeftMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * -10;
            playerContainer.offset += transform.right * 10;

        }
    }
    private void moveRight()
    {
        Transform move = targetWayPoint.GetComponent<Waypoint>().getRightMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * 10;
            playerContainer.offset += transform.right * -10;
        }
    }
    public bool hasReachedTarget()
    {
        return transform.position == targetWayPoint.transform.position;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            takeDamage();
            print("Hit obstacle");
        }
    }

    public void takeDamage()
    {
        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            PlayerPrefs.SetInt("Score", (int)score);
            GameManager.Instance.latestScore = (int)score;
            SceneManager.LoadScene(2);
        }
    }
}
public enum PlayerStance
{
    low, medium, high, idle
}
