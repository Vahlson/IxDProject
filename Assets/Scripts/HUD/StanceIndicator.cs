using Unity;
using UnityEngine;
using UnityEngine.UI;
class StanceIndicator : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private Image _stance;
    [SerializeField]
    private Image _stanceDuration;
    private float _stanceTime = 0.0f;
    private float duration = 0.0f;
    private bool _isIdle = true;
    [SerializeField]
    private Sprite highSprite;
    [SerializeField]
    private Sprite mediumSprite;
    [SerializeField]
    private Sprite lowSprite;
    [SerializeField]
    private Sprite idleSprite;



    void Start()
    {
        _animator = GetComponent<Animator>();
        _stance.sprite = idleSprite;

    }
    void Update()
    {

        if (!_isIdle)
        {
            duration -= Time.deltaTime;
            updateStanceDuration();

        }
        else
        {
            _stanceDuration.fillAmount = 1;
        }

    }
    void updateStanceDuration()
    {
        _stanceDuration.fillAmount = duration / _stanceTime;
    }
    public void setIdle()
    {
        _isIdle = true;
        _stance.sprite = idleSprite;
    }
    public void setMedium(float duration)
    {
        this.duration = duration;
        this._stanceTime = duration;
        _isIdle = false;
        _stance.sprite = mediumSprite;

    }
    public void setLow(float duration)
    {
        this.duration = duration;
        this._stanceTime = duration;
        _isIdle = false;
        _stance.sprite = lowSprite;
    }
    public void setHigh(float duration)
    {
        this.duration = duration;
        this._stanceTime = duration;
        _isIdle = false;
        _stance.sprite = highSprite;
    }

}