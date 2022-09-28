using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Animator _animator;
    public float velocity = 5.0f;
    public float acceleration = .2f;
    int velocityHash;
    public Transform targetWayPoint;
    [SerializeField] private int maxHealth = 10;

    [SerializeField] private int jumpSpeed = 4000;
    public int currentHealth;

    [HideInInspector] public float score;
    Rigidbody m_Rigidbody;




    void Awake()
    {
        currentHealth = maxHealth;

    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        _animator.SetBool("Jump", false);
        m_Rigidbody = GetComponent<Rigidbody>();
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

    private void jump()
    {
        _animator.SetBool("Jump", true);
        _animator.SetBool("Jump", false);
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
        }
    }
    private void moveRight()
    {
        Transform move = targetWayPoint.GetComponent<Waypoint>().getRightMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * 10;
        }
    }
    public bool hasReachedTarget()
    {
        return transform.position == targetWayPoint.transform.position;
    }


    /* void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            takeDamage();
            print("Hit obstacle");
        }
    } */


    public void takeDamage()
    {
        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
        print(currentHealth);
    }

    public void avoidObstacale(Obstacles.BlockadeType blockadeType)
    {
        print(blockadeType);

        if (blockadeType == Obstacles.BlockadeType.High)
        {

            print("slide");
            _animator.SetBool("Slide", true);

        }

        else if (blockadeType == Obstacles.BlockadeType.Low)
        {
            print("jumping");
            _animator.SetBool("Jump", true);
            transform.position += transform.up * 4;
            transform.position += transform.forward;
            //m_Rigidbody.AddForce(transform.up * jumpSpeed);



        }
        else if (blockadeType == Obstacles.BlockadeType.Full)
        {
            _animator.SetBool("Kick", true);
            print("kick");
        }

    }

    public void keepRunning()
    {
        _animator.SetBool("Slide", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Kick", false);

    }
}
