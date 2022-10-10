using UnityEngine;

class Badguy : MonoBehaviour
{
    private Animator _animator;
    public float velocity = 5.0f;
    private float _animationMultiplier = 2.0f;
    public Transform targetWayPoint;
    private int _animationMultiplierHash;
    private int _velocityHash;
    public float acceleration = 0.5f;

    [SerializeField]
    private Player _target;
    void OnLaneSwitched(string direction)
    {
        print("Onlaneswitched");
        if (direction == "Left")
        {
            Transform move = targetWayPoint.GetComponent<Waypoint>().getLeftMove();
            targetWayPoint = move;
            transform.position += transform.right * -5;

        }
        else if (direction == "Right")
        {
            Transform move = targetWayPoint.GetComponent<Waypoint>().getRightMove();
            targetWayPoint = move;
            transform.position += transform.right * 5;
        }
    }
    void Start()
    {
        _target.OnLaneSwitched += OnLaneSwitched;
        _animator = GetComponent<Animator>();
        _velocityHash = Animator.StringToHash("Velocity");
        _animationMultiplierHash = Animator.StringToHash("AnimationMultiplier");

    }
    void OnDestroy()
    {
        _target.OnLaneSwitched -= OnLaneSwitched;

    }
    void Update()
    {
        _animator.SetFloat(_velocityHash, velocity);
        _animator.SetFloat(_animationMultiplierHash, velocity / 4);
        if (targetWayPoint != null)
        {
            moveToWaypoint();
        }
        velocity += Time.deltaTime * acceleration;

    }
    private void moveToWaypoint()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, targetWayPoint.position - transform.position, velocity * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, velocity * Time.deltaTime);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (other is BoxCollider)
            {
                print("game over");
            }
        }

    }

}