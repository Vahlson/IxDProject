using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAnimationBlender : MonoBehaviour
{
    private Animator animator;
    private float blendValue = 0.0f;
    private float targetBlendValue = 0.0f;
    private float previousTargetBlendValue = 0.0f;



    // starting value for the Lerp
    private float t = 0.0f;
    [SerializeField] private float blendSpeed = 0.5f;
    [SerializeField] private float changeAnimationChance = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        blendValue = Mathf.Lerp(previousTargetBlendValue, targetBlendValue, t);
        t += blendSpeed * Time.deltaTime;
        //If we have reached the target
        if (t > 1.0f)
        {
            t = 0.0f;

            previousTargetBlendValue = targetBlendValue;
            targetBlendValue = Random.Range(0.0f, 10.0f);


        }

        animator.SetFloat("Blend", blendValue);
    }

    public void playDeathAnimation()
    {
        animator.SetBool("IsDead", true);
    }
}
