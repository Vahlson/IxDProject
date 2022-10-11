using UnityEngine;

class Badguy : MonoBehaviour
{
    private Animator _animator;
    public float velocity = 0;
    private int _velocityHash;
    [SerializeField]
    float totalDistanceTravelled = 0.0f;

    [SerializeField]
    private Player _target;
    public float badGuyAcceleration = 0f;


    void Start()
    {
        badGuyAcceleration = _target.baseAcceleration *1.3f;
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
            if (GameManager.Instance.gameState != GameState.over)
            {
                _target.gotCaught();
                transform.position = _target.transform.position - _target.transform.forward * 5;
                GameManager.Instance.gameState = GameState.over;
            }
            else
            {

                transform.forward = Vector3.RotateTowards(transform.forward, _target.transform.position - transform.position, 3 * Time.deltaTime, 0.0f);
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, 3 * Time.deltaTime);
                print("gameover");
                //activate kick, complete kick, game over.
            }
        }
        velocity += Time.deltaTime * badGuyAcceleration;

    }
}