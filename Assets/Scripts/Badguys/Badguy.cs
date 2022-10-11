using UnityEngine;

class Badguy : MonoBehaviour
{
    private Animator _animator;
    private float velocity = 0;
    public float badGuyAccelerationFactor = 1.3f;
    [SerializeField]
    float totalDistanceTravelled = 0.0f;
    private int _velocityHash;
    [SerializeField]
    private Player _target;
    public float acceleration = 0f;
    private Vector3 endPosition;
    private bool targetCaught = false;
    [SerializeField]
    private SkinnedMeshRenderer mesh;


    void Start()
    {
        mesh.enabled = false;
        acceleration = _target.baseAcceleration * badGuyAccelerationFactor;
        totalDistanceTravelled = -Vector3.Distance(transform.position, _target.transform.position);
        velocity = _target.velocity;
        _animator = GetComponent<Animator>();
        _velocityHash = Animator.StringToHash("Velocity");

    }

    void Update()
    {
        _animator.SetFloat(_velocityHash, velocity);
        totalDistanceTravelled += velocity * Time.deltaTime;
        if (_target.totalDistanceTravelled < this.totalDistanceTravelled || GameManager.Instance.gameState == GameState.over)
        {
            if (!targetCaught)
            {
                mesh.enabled = true;
                transform.position = _target.transform.position - _target.transform.forward * 5;
                _target.gotCaught();
                GameManager.Instance.gameState = GameState.over;
                targetCaught = true;
            }

            {
                if (endPosition == Vector3.zero)
                {
                    endPosition = _target.transform.position;
                }
                transform.forward = Vector3.RotateTowards(transform.forward, endPosition - transform.position, 3 * Time.deltaTime, 0.0f);
                transform.position = Vector3.MoveTowards(transform.position, endPosition, 3 * Time.deltaTime);
            }
        }
        velocity += Time.deltaTime * acceleration;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            _animator.SetBool("Kick", true);
            _animator.SetBool("GameOver", true);
        }
    }

}