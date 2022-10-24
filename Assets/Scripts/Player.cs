using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Cinemachine;
public class Player : MonoBehaviour
{
    public PlayerContainer playerContainer;
    private ArduinoInputController arduinoInputController;
    private Animator _animator;

    //Movement speed -------------------------------------
    public float velocity = 20.0f;
    public float onDamageVelocityMultiplier = 0.8f;




    [SerializeField]
    private float bpmFactor = 0.01f;

    //private Queue<float> steps = new Queue<float>();
    //private float bpmAcceleration = 0.0f;

    //[SerializeField] private float keepBpmTime = 5f;

    //Game Data ----------------------------------------------------
    [SerializeField]
    private GameObject _background;
    private Quaternion _backgroundRotation;
    int velocityHash;
    public Transform targetWayPoint;
    [SerializeField] private int maxHealth = 10;
    public int currentHealth;
    [HideInInspector] public float score;
    [SerializeField] private int pointsForObstacle = 50;

    private Vector3 previousLocation;

    //Lane Switching -----------------------------------------------------------------------
    private float targetLaneTargetOffset = 0;
    private float targetLaneOffset = 0;
    [SerializeField] private float laneSwitchTime = 1f;
    private float laneSwitchTimeElapse;
    private Vector3 moveToPosition;

    //Camera follow -------------------------------------------------------------
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

    //Obstacles and dodging --------------------------------------------------------------------------
    Obstacles lastObstacle = null;

    bool jump;
    bool slide;
    bool kick;
    public bool damage;
    public event Action<PlayerStance> OnStanceChanged;
    public event Action<String> OnLaneSwitched;
    public event Action<float> OnObstacleDodged;

    //Animation and sound ------------------------------------------------------------------------
    private string jumpAnimation;
    private PlayerStance _playerStance = PlayerStance.idle;
    private int _animationMultiplierHash;
    public float stanceDuration = 8.0f;
    public float currentStanceDuration;
    [SerializeField]
    public float totalDistanceTravelled = 0.0f;
    // public AudioClip kickSound;
    // public AudioClip jumpSound;
    // public AudioClip slideSound;
    public AudioClip scoreSound;
    public AudioClip damageSound;
    public AudioClip kickSound;
    public AudioClip jumpSound;

    [SerializeField] private AudioClip gotKickedSound;
    [SerializeField] private AudioClip painSound;

    private AudioSource _audioSource;

    [SerializeField] ParticleSystem dustTrail;



    void Awake()
    {
        _backgroundRotation = this.transform.rotation;
        currentHealth = maxHealth;

    }
    void Start()
    {
        if (!GameManager.Instance.useArduinoInput)
        {
            FindObjectOfType<SerialController>().enabled = false;
        }
        _audioSource = GetComponent<AudioSource>();
        currentStanceDuration = stanceDuration;
        _animator = transform.GetChild(0).GetComponent<Animator>();
        arduinoInputController = GetComponent<ArduinoInputController>();
        velocityHash = Animator.StringToHash("Velocity");
        _animationMultiplierHash = Animator.StringToHash("AnimationMultiplier");
        _animator.SetBool("Jump", false);
        laneSwitchTimeElapse = laneSwitchTime;

        startCameraLerpBetweenLanes(0);
        damage = true;

        GameManager.Instance.increasingMinVelocity = 2f;
        //initSteps();
    }



    void step()
    {
        GameManager.Instance.step();
    }
    /* void recountBPM()
    {
        //print(steps.Count);
        GameManager.Instance.recountBPM();
        bpmAcceleration = getBPM() * bpmFactor;
    }
    public int getBPM()
    {
        return GameManager.Instance.getBPM();
    } */
    void Update()
    {
        if (_playerStance != PlayerStance.idle)
        {
            currentStanceDuration -= Time.deltaTime;
            if (currentStanceDuration <= 0)
            {
                setStance(PlayerStance.idle);
            }
        }
        _animator.SetFloat(velocityHash, velocity);
        _animator.SetFloat(_animationMultiplierHash, velocity / 6);
        //recountBPM();





        if (targetWayPoint != null)
        {
            moveToWaypoint();
        }

        if (GameManager.Instance.gameState == GameState.ongoing && GameManager.Instance.useArduinoInput)
        {


            if (arduinoInputController.getKeyDown(5) || arduinoInputController.getKeyDown(4))
            {
                step();
            }
            if (arduinoInputController.getKeyDown(3))
            {
                moveRight();
            }
            if (arduinoInputController.getKeyDown(6))
            {
                moveLeft();
            }
            if (arduinoInputController.getKeyDown(0))
            {
                setStance(PlayerStance.low);
            }
            if (arduinoInputController.getKeyDown(1))
            {
                setStance(PlayerStance.medium);
            }
            if (arduinoInputController.getKeyDown(2))
            {
                setStance(PlayerStance.high);
            }

        }

        if (GameManager.Instance.gameState == GameState.ongoing)
        {
            if (Input.GetKeyUp("j") || Input.GetKeyUp("h"))
            {
                step();
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
                print("LOW");
            }
            if (Input.GetKeyUp("o"))
            {
                setStance(PlayerStance.medium);
            }
            if (Input.GetKeyUp("p"))
            {
                setStance(PlayerStance.high);
            }

        }

        if (GameManager.Instance.gameState == GameState.ongoing)
        {
            totalDistanceTravelled += Time.deltaTime * velocity;
            //Score is based on distance, so higher speeds is rewarded.
            score += Vector3.Distance(transform.position, previousLocation);
            previousLocation = transform.position;

            GameManager gm = GameManager.Instance;
            //velocity += Time.deltaTime * ((gm.tramplingSpeed / gm.maxTramplingSpeed - gm.increaseSpeedFromTramplingThreshold) * gm.tramplingSpeedFactor);
            velocity = gm.increasingMinVelocity + gm.tramplingSpeed * gm.tramplingSpeedFactor;
            //velocity = Mathf.Clamp(velocity, gm.increasingMinVelocity, float.MaxValue);
            //velocity *= ;
        }

        if (cameraLerpTime < (1 - 0.01f))
        {
            cameraLerpTime += Time.deltaTime * cameraLerpSpeed;
            cameraFollowOffset = Mathf.Lerp(cameraFollowLerpStart, cameraFollowTargetOffset, cameraLerpTime);
            //print("Camera Offset" + cameraFollowOffset);

            cameraFollowTransform.localPosition = new Vector3(cameraFollowOffset, cameraFollowTransform.localPosition.y, cameraFollowTransform.localPosition.z);
        }
        else
        {
            oldCameraFollowTargetOffset = cameraFollowTargetOffset;
        }


        if (jump)
        {
            jump = false;
        }

        else if (slide)
        {
            slide = false;
        }

        else if (kick)
        {
            kick = false;
        }
    }
    void setStance(PlayerStance playerStance)
    {
        keepRunning();
        this._playerStance = playerStance;
        OnStanceChanged?.Invoke(_playerStance);
        currentStanceDuration = stanceDuration;


    }

    void FixedUpdate()
    {

    }

    void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        _background.transform.rotation = _backgroundRotation;
    }

    private void moveToWaypoint()
    {
        //The old, working version.
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);

        //2.5 is half of the waypoint length
        //Getting the offset of the component that is forward between waypoint and player. so if z is of forward, what is their z offset.
        // float playerToTargetForwardOffset = Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) - Vector3.Dot(targetWayPoint.forward, transform.position);
        // moveToPosition = targetWayPoint.position + targetWayPoint.forward * (2.5f - playerToTargetForwardOffset);

        // //print("movetoPos: " + moveToPosition);
        // //print("targetWaypoinyPos: " + targetWayPoint.position);

        // //Old smooth curve version
        // //transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        // //transform.position = Vector3.MoveTowards(transform.position, moveToPosition, velocity * Time.deltaTime);

        // //This is used to take account for the speed forward becoming slower when switching lanes which we dont want
        // // print("reached lane? : " + hasReachedLane());
        // float laneSwitchSpeedAdjustmentFactor = hasReachedLane() ? 1 : Mathf.Sqrt(2);
        // transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.forward, rotationSpeed * Time.deltaTime, 0.0f);
        // transform.position = Vector3.MoveTowards(transform.position, moveToPosition, laneSwitchSpeedAdjustmentFactor * velocity * Time.deltaTime);
        // ////////////////////////////
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

        //print("Start Offset: " + cameraFollowStartOffset);
        //baseOffsetToCenter - oldCameraFollowTargetOffset
        cameraFollowStartOffset += baseOffsetToCenter;
        cameraFollowLerpStart = cameraFollowStartOffset - oldCameraFollowStartOffset + oldCameraFollowTargetOffset;


        //print("Lerp start " + cameraFollowLerpStart);
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
            //playerContainer.offset += transform.right * 5;

            //cameraFollowTransform.position += transform.right * 5;


            //transform.position += transform.right * -5;

            startCameraLerpBetweenLanes(5);
            OnLaneSwitched?.Invoke("Left");
            //transform.position += transform.right * -5;
            // startLerpBetweenLanes(-5);

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
            //playerContainer.offset += transform.right * -5;

            //cameraFollowTransform.position += transform.right * -5;

            OnLaneSwitched?.Invoke("Right");
            //transform.position += transform.right * 5;
            startCameraLerpBetweenLanes(-5);
        }
    }
    public bool hasReachedTarget()
    {
        return Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetWayPoint.transform.position.x, 0, targetWayPoint.transform.position.z)) < 0.2;
        // return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        //print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));

        // return Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position) >= 0;
    }

    public bool hasReachedLane()
    {
        //return transform.position == targetWayPoint.transform.position;
        //return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

        //print("Target forward offset: " + (Vector3.Dot(targetWayPoint.forward, transform.position) - Vector3.Dot(targetWayPoint.forward, targetWayPoint.position)));
        float playerToLaneSideOffset = Vector3.Dot(targetWayPoint.right, targetWayPoint.position) - Vector3.Dot(targetWayPoint.right, transform.position);
        return playerToLaneSideOffset <= 0.1;
    }


    //return transform.position == targetWayPoint.transform.position;
    //return Vector3.Distance(transform.position, moveToPosition) <= 0.3;

    public void takeDamage(Obstacles.BlockadeType blockadeType)
    {
        if ((blockadeType == Obstacles.BlockadeType.High && _playerStance == PlayerStance.high) ||
        (blockadeType == Obstacles.BlockadeType.Full && _playerStance == PlayerStance.medium) ||
        (blockadeType == Obstacles.BlockadeType.Low && _playerStance == PlayerStance.low) ||
        (blockadeType == Obstacles.BlockadeType.Ramp && _playerStance == PlayerStance.low))
        {
            return;
        }
        else
        {
            //velocity *= onDamageVelocityMultiplier;
            GameManager.Instance.slowDownTrampling();
            currentHealth -= 1;
            _audioSource.clip = damageSound;
            _audioSource.Play();
            print("GOT HIT");
            _animator.SetBool("GotHit", true);
            //_animator.SetBool("GotHit", false);


            //The player looses entirely if he/she cant dodge the train
            if (blockadeType == Obstacles.BlockadeType.Ramp)
            {
                currentHealth = 0;
            }

            if (currentHealth <= 0)
            {
                GameManager.Instance.gameState = GameState.over;
            }
        }
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("Score", (int)score);
        GameManager.Instance.latestScore = (int)score;
        GameManager.Instance.gameState = GameState.over;
        SceneManager.LoadScene(2);
    }

    public void avoidObstacle(Obstacles obstacle)
    {
        //print(obstacle.blockadeType);

        if (obstacle == lastObstacle) return;
        lastObstacle = obstacle;

        if (obstacle.blockadeType == Obstacles.BlockadeType.High && _playerStance == PlayerStance.high)
        {
            damage = false;
            slide = true;
            _animator.SetBool("Slide", true);
            DodgedObstacle();

            //Play sound effect
            /* _audioSource.clip = kickSound;
            _audioSource.Play(); */

        }

        else if (obstacle.blockadeType == Obstacles.BlockadeType.Low && _playerStance == PlayerStance.low)
        {
            damage = false;
            jump = true;
            _animator.SetBool("Jump", true);
            DodgedObstacle();

            _audioSource.clip = jumpSound;
            _audioSource.Play();

            print("YO WE JUMPING");

            //Play sound effect
            /* _audioSource.clip = kickSound;
            _audioSource.Play(); */


        }
        else if (obstacle.blockadeType == Obstacles.BlockadeType.Full && _playerStance == PlayerStance.medium)
        {
            damage = false;
            kick = true;
            _animator.SetBool("Kick", true);
            DodgedObstacle();
            ObstacleAnimationBlender ob = obstacle.gameObject.GetComponentInChildren<ObstacleAnimationBlender>();

            //Play sound effect
            _audioSource.clip = kickSound;
            _audioSource.Play();

            //try to play the death animation if existing
            if (ob != null)
            {
                ob.playDeathAnimation();
            }
            else
            {
                print("No human found");
            }

        }
        else if (obstacle.blockadeType == Obstacles.BlockadeType.Ramp && _playerStance == PlayerStance.low)
        {
            damage = false;
            //jump = true;
            _animator.SetBool("JumpHigh", true);
            DodgedObstacle();
            print("RAMP");
            _audioSource.clip = jumpSound;
            _audioSource.Play();

        }

    }
    private void DodgedObstacle()
    {
        //_audioSource.clip = scoreSound;
        //print("playing scoresound");
        StartCoroutine(GameManager.Instance.playAudioOnGameManager(scoreSound));
        //_audioSource.Play();
        score += pointsForObstacle;
        OnObstacleDodged?.Invoke(pointsForObstacle);
    }

    public void keepRunning()
    {
        _animator.SetBool("Slide", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Kick", false);
        _animator.SetBool("JumpHigh", false);
        _animator.SetBool("GotHit", false);
        jump = false;
        slide = false;
        kick = false;
        damage = true;
    }
    public void gotCaught()
    {
        keepRunning();
        //GameManager.Instance.steps.Clear();
        currentHealth = 0;
        GameManager.Instance.tramplingSpeed = 0;
        //this.bpmAcceleration = 0;
        this.velocity = 0;
        GameManager.Instance.increasingMinVelocity = 0;
        CinemachineVirtualCamera cm = GameObject.FindGameObjectWithTag("FollowCam").GetComponent<CinemachineVirtualCamera>();
        cm.Follow = null;
        cm.LookAt = null;
        this.transform.forward = -transform.forward;
        keepRunning();
        _animator.SetBool("GameOver", true);
        _animator.SetBool("Death", true);
        Invoke("GameOver", 3.0f);

        List<AudioClip> defeatSounds = new List<AudioClip>();
        defeatSounds.Add(gotKickedSound);
        defeatSounds.Add(painSound);

        StartCoroutine(GameManager.Instance.playAudioSequentially(_audioSource, defeatSounds, delayBefore: 1.2f));

    }

}
public enum PlayerStance
{
    low, medium, high, idle
}
