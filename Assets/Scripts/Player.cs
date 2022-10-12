using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
    public PlayerContainer playerContainer;
    private Animator _animator;
    public float velocity = 5.0f;
    public float rotationSpeed = 3.0f;
    public float acceleration = .2f;
    int velocityHash;
    public Transform targetWayPoint;
    [SerializeField] private int maxHealth = 10;

    [SerializeField] private int jumpSpeed = 70;
    public int currentHealth;
    [HideInInspector] public float score;
    public PlayerStance stance = PlayerStance.high;

    //Lane Switching
    private float targetLaneTargetOffset = 0;
    private float targetLaneOffset = 0;
    [SerializeField] private float laneSwitchTime = 1f;
    private float laneSwitchTimeElapse;

    private Vector3 moveToPosition;

    public Transform cameraFollowTransform;
    [Range(0f, 5f)] public float cameraMoveWithLaneSwitch = 2f;
    private float cameraFollowTargetOffset = 0f;
    private float oldCameraFollowTargetOffset = 0f;
    private float oldCameraFollowStartOffset = 0f;
    private float cameraFollowStartOffset = 0f;
    private float cameraFollowOffset = 0f;
    private float cameraLerpTime = 0f;
    public float cameraLerpSpeed = 0.5f;

    private float cameraFollowLerpStart = 0f;
    //public float cameraTargetOffset = 1f;


    Obstacles lastObstacle = null;

    bool jump;
    bool slide;
    bool kick;
    public event Action<PlayerStance> OnStanceChanged;
    private string jumpAnimation;
    private PlayerStance _playerStance = PlayerStance.idle;
    void Awake()
    {
        currentHealth = maxHealth;

    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        _animator.SetBool("Jump", false);
        laneSwitchTimeElapse = laneSwitchTime;

        startCameraLerpBetweenLanes(0);
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
        if (Input.GetKeyUp("i"))
        {
            setStance(PlayerStance.low);
        }
        if (Input.GetKeyUp("o"))
        {
            setStance(PlayerStance.medium);
        }
        if (Input.GetKeyUp("p"))
        {
            setStance(PlayerStance.high);
        }
        score += Time.deltaTime;
        velocity += Time.deltaTime * acceleration;

        //Lerping camera offset
        //Also adding a short bias
        if (cameraLerpTime < (1 - 0.01f))
        {
            cameraLerpTime += Time.deltaTime * cameraLerpSpeed;
            cameraFollowOffset = Mathf.Lerp(cameraFollowLerpStart, cameraFollowTargetOffset, cameraLerpTime);
            print("Camera Offset" + cameraFollowOffset);

            cameraFollowTransform.localPosition = new Vector3(cameraFollowOffset, cameraFollowTransform.localPosition.y, cameraFollowTransform.localPosition.z);
        }
        else
        {
            oldCameraFollowTargetOffset = cameraFollowTargetOffset;
        }

        /* if (laneSwitchTimeElapse < laneSwitchTime && laneSwitchTime != 0)
        {
            targetLaneOffset = Mathf.Lerp(0, targetLaneTargetOffset, laneSwitchTimeElapse / laneSwitchTime);
            laneSwitchTimeElapse += Time.deltaTime;
            //transform.position += transform.right * targetLaneOffset;
        } */

    }
    void setStance(PlayerStance playerStance)
    {
        this._playerStance = playerStance;
        OnStanceChanged?.Invoke(_playerStance);

    }

    void FixedUpdate()
    {
        if (jump)
        {
            // m_Rigidbody.AddForce(transform.up * jumpSpeed);



            //transform.position += transform.up * 3;
            jump = false;
        }


        else if (slide)
        {
            // m_Rigidbody.AddForce(-transform.up * 12);
            slide = false;
        }

        else if (kick)
        {
            // m_Rigidbody.AddForce((-transform.up) * 6);
            kick = false;
        }
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    private void moveToWaypoint()
    {
        //The old, working version.
        //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        //transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);

        //2.5 is half of the waypoint length
        //Getting the offset of the component that is forward between waypoint and player. so if z is of forward, what is their z offset.
        float playerToTargetForwardOffset = Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) - Vector3.Dot(targetWayPoint.forward, transform.position);
        moveToPosition = targetWayPoint.position + targetWayPoint.forward * (2.5f - playerToTargetForwardOffset);

        //print("movetoPos: " + moveToPosition);
        //print("targetWaypoinyPos: " + targetWayPoint.position);

        //Old smooth curve version
        //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        //transform.position = Vector3.MoveTowards(transform.position, moveToPosition, velocity * Time.deltaTime);

        //This is used to take account for the speed forward becoming slower when switching lanes which we dont want
        //print("reached lane? : " + hasReachedLane());
        float laneSwitchSpeedAdjustmentFactor = hasReachedLane() ? 1 : Mathf.Sqrt(2);
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, moveToPosition, laneSwitchSpeedAdjustmentFactor * velocity * Time.deltaTime);
        ////////////////////////////
        /* Vector3 newTargetWaypoint = transform.position;
        Vector3 moveToPositionForwardOffset = Vector3.Scale(targetWayPoint.forward, moveToPosition);
        List<float> vectorArray = new List<float>();
        vectorArray.Add(targetWayPoint.forward.x);
        vectorArray.Add(targetWayPoint.forward.y);
        vectorArray.Add(targetWayPoint.forward.z);
        

        int forwardIndex = vectorArray.IndexOf(1);
        forwardIndex = forwardIndex == -1 ? vectorArray.IndexOf(-1) : 1;
        print("targetWayPoint.forward: " + targetWayPoint.forward + "forward index:" + forwardIndex + " vectorArray: " + vectorArray[0] + "," + vectorArray[1] + "," + vectorArray[2]);
        newTargetWaypoint[forwardIndex] = moveToPositionForwardOffset[forwardIndex];
        //newTargetWaypoint = newTargetWaypoint targetWayPoint.forward * moveToPositionForwardOffset;

        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, newTargetWaypoint, velocity * Time.deltaTime); */

        //offset for lane switch

        /* if (laneSwitchTimeElapse < laneSwitchTime && laneSwitchTime != 0)
{
    targetLaneOffset = Mathf.Lerp(0, targetLaneTargetOffset, laneSwitchTimeElapse / laneSwitchTime);
    laneSwitchTimeElapse += Time.deltaTime;
    //print("HEJJJJJJJJJJJJ");

    print("Offset: " + targetLaneOffset);
    print("LaneSwitchOffset: " + targetLaneOffset);
    //transform.position += transform.right * targetLaneOffset;
}
*/
        /* Vector3 newLaneSwitchTarget = transform.position;
              Vector3 moveToPositionSideOffset = Vector3.Scale(targetWayPoint.right, moveToPosition);
              List<float> vectorArray2 = new List<float>();
              vectorArray2.Add(targetWayPoint.right.x);
              vectorArray2.Add(targetWayPoint.right.y);
              vectorArray2.Add(targetWayPoint.right.z);
              int forwardIndex2 = vectorArray2.IndexOf(1f);
              newLaneSwitchTarget[forwardIndex2] = moveToPositionSideOffset[forwardIndex2]; */


        //transform.position = transform.right * targetLaneOffset;
        //transform.position = Vector3.Lerp(transform.position, transform.right * targetLaneOffset, 0.5f * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, newLaneSwitchTarget, velocity * Time.deltaTime);

    }

    private void startCameraLerpBetweenLanes(float baseOffsetToCenter)
    {
        //Jump camera follow to center to keep centered

        //cameraFollowTransform.position += transform.right * baseOffsetToCenter;
        //cameraFollowTransform.localPosition = new Vector3(transform.localPosition.x + baseOffsetToCenter, cameraFollowTransform.localPosition.y, cameraFollowTransform.localPosition.z);

        //Reset cameraOffset
        cameraFollowOffset = 0f;


        //cameraFollowTransform.localPosition = new Vector3(cameraFollowStartOffset, cameraFollowTransform.localPosition.y, cameraFollowTransform.localPosition.z);

        print("Start Offset: " + cameraFollowStartOffset);
        //baseOffsetToCenter - oldCameraFollowTargetOffset
        cameraFollowStartOffset += baseOffsetToCenter;
        cameraFollowLerpStart = cameraFollowStartOffset - oldCameraFollowStartOffset + oldCameraFollowTargetOffset;


        print("Lerp start " + cameraFollowLerpStart);
        //Move from middle a little bit.
        float followOffset = cameraFollowStartOffset == 0 ? 0 : cameraMoveWithLaneSwitch * Math.Sign(baseOffsetToCenter);
        cameraFollowTargetOffset = cameraFollowStartOffset - followOffset;

        oldCameraFollowTargetOffset = cameraFollowTargetOffset;
        oldCameraFollowStartOffset = cameraFollowStartOffset;
        cameraLerpTime = 0;
    }

    private void moveLeft()
    {
        // if (hasReachedLane() != true) return;
        Transform move = targetWayPoint.GetComponent<Waypoint>().getLeftMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * -5;
            playerContainer.offset += transform.right * 5;

            //cameraFollowTransform.position += transform.right * 5;


            //transform.position += transform.right * -5;

            startCameraLerpBetweenLanes(5);

        }
    }
    private void moveRight()
    {
        // if (hasReachedLane() != true) return;
        Transform move = targetWayPoint.GetComponent<Waypoint>().getRightMove();
        if (move != null)
        {
            targetWayPoint = move;
            transform.position += transform.right * 5;
            playerContainer.offset += transform.right * -5;

            //cameraFollowTransform.position += transform.right * -5;

            //transform.position += transform.right * 5;
            startCameraLerpBetweenLanes(-5);
        }
    }
    public bool hasReachedTarget()
    {
        //return transform.position == targetWayPoint.transform.position;
        //return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        //print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));

        return Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) >= 0;
    }

    public bool hasReachedLane()
    {
        //return transform.position == targetWayPoint.transform.position;
        //return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        //print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));
        float playerToLaneSideOffset = Vector3.Dot(targetWayPoint.right, targetWayPoint.position) - Vector3.Dot(targetWayPoint.right, transform.position);
        return playerToLaneSideOffset <= 0.1;
    }


    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.tag == "Obstacle")
    //     {
    //         takeDamage();
    //         print("Hit obstacle");
    //     }
    // }


    public void takeDamage(Obstacles.BlockadeType blockadeType)
    {
        if ((blockadeType == Obstacles.BlockadeType.High && _playerStance == PlayerStance.high) ||
        (blockadeType == Obstacles.BlockadeType.Full && _playerStance == PlayerStance.medium) ||
        (blockadeType == Obstacles.BlockadeType.Low && _playerStance == PlayerStance.low))
        {
            return;
        }
        else
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


    public void avoidObstacle(Obstacles obstacle)
    {
        print(obstacle.blockadeType);
        if (obstacle == lastObstacle) return;
        lastObstacle = obstacle;

        if (obstacle.blockadeType == Obstacles.BlockadeType.High && _playerStance == PlayerStance.high)
        {

            //print("slide");
            slide = true;
            _animator.SetBool("Slide", true);
        }

        else if (obstacle.blockadeType == Obstacles.BlockadeType.Low && _playerStance == PlayerStance.low)
        {
            //print("jumping");
            jump = true;
            _animator.SetBool("Jump", true);


        }
        else if (obstacle.blockadeType == Obstacles.BlockadeType.Full && _playerStance == PlayerStance.medium)
        {
            kick = true;
            _animator.SetBool("Kick", true);
            ObstacleAnimationBlender ob = obstacle.gameObject.GetComponentInChildren<ObstacleAnimationBlender>();
            //try to play the death animation if existing
            if (ob != null)
            {
                ob.playDeathAnimation();
            }
            else
            {
                print("No human found");
            }
            // print("kick");
        }

    }

    public void keepRunning()
    {

        //print("Floor");
        //_animator.ResetTrigger("Jumping");
        _animator.SetBool("Slide", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Kick", false);

        jump = false;
        slide = false;
        kick = false;

    }
}
public enum PlayerStance
{
    low, medium, high, idle
}
