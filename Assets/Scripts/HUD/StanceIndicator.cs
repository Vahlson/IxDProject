using Unity;
using UnityEngine;
class StanceIndicator : MonoBehaviour
{
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();

    }
    public void setIdle()
    {
        _animator.SetBool("medium", false);
        _animator.SetBool("low", false);
        _animator.SetBool("high", false);
    }
    public void setMedium()
    {
        _animator.SetBool("medium", true);
        _animator.SetBool("low", false);
        _animator.SetBool("high", false);
    }
    public void setLow()
    {
        _animator.SetBool("medium", false);
        _animator.SetBool("low", true);
        _animator.SetBool("high", false);


    }
    public void setHigh()
    {
        _animator.SetBool("medium", false);
        _animator.SetBool("low", false);
        _animator.SetBool("high", true);

    }

}