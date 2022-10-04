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

    [SerializeField] private int jumpSpeed = 70;
    public int currentHealth;

    [HideInInspector] public float score;
    Rigidbody m_Rigidbody;

    Obstacles lastObstacle = null;

    bool jump;
    bool slide;
    bool kick;

    private string jumpAnimation;





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

    void FixedUpdate()
    {
        if (jump)
        {
            m_Rigidbody.AddForce(transform.up * jumpSpeed);



            //transform.position += transform.up * 3;
            jump = false;
        }


        else if (slide)
        {
            m_Rigidbody.AddForce(-transform.up * 12);
            slide = false;
        }

        else if (kick)
        {
            m_Rigidbody.AddForce((-transform.up) * 6);
            kick = false;
        }
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
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


    public void avoidObstacale(Obstacles obstacle)
    {
        print(obstacle.blockadeType);
        if (obstacle == lastObstacle) return;
        lastObstacle = obstacle;

        if (obstacle.blockadeType == Obstacles.BlockadeType.High)
        {

            print("slide");
            slide = true;
            _animator.SetBool("Slide", true);


        }

        else if (obstacle.blockadeType == Obstacles.BlockadeType.Low)
        {
            print("jumping");
            jump = true;
            _animator.SetBool("Jump", true);


        }
        else if (obstacle.blockadeType == Obstacles.BlockadeType.Full)
        {
            kick = true;
            _animator.SetBool("Kick", true);

            print("kick");
        }

    }

    public void keepRunning()
    {

        print("Floor");
        //_animator.ResetTrigger("Jumping");
        _animator.SetBool("Slide", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Kick", false);

        jump = false;
        slide = false;
        kick = false;

    }
}
