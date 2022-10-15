using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAnimationBlender : MonoBehaviour
{
    private Animator animator;
    private float blendValue = 0.0f;
    private float targetBlendValue = 0.0f;
    private float previousTargetBlendValue = 0.0f;

    public float blendMaxValue = 10f;
    [Tooltip("The largest value you set in the controller for transitioning to a small action. How you distribute the range of [0 - LargestSmallActionValue] governs how often the different actions are performed when any action is to be performed.")]
    public float largestSmallActionValue = 12f;
    public float minTimeBetweenSmallActions = 8;
    public float maxTimeBetweenSmallActions = 15;
    private float targetTimeUntilAction;
    private float timeSinceLastAction = 0f;

    // starting value for the Lerp
    private float t = 0.0f;
    [SerializeField] private float blendSpeed = 0.5f;
    [SerializeField] private float changeAnimationChance = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        targetTimeUntilAction = Random.Range(minTimeBetweenSmallActions, maxTimeBetweenSmallActions);
    }

    // Update is called once per frame
    void Update()
    {

        //BLENDING IN TREE
        blendValue = Mathf.Lerp(previousTargetBlendValue, targetBlendValue, t);
        t += blendSpeed * Time.deltaTime;
        //If we have reached the target
        if (t > 1.0f)
        {
            t = 0.0f;

            previousTargetBlendValue = targetBlendValue;
            targetBlendValue = Random.Range(0.0f, blendMaxValue);


        }

        animator.SetFloat("Blend", blendValue);


        //ADDING SMALL ACTIONS IN IDLE FROM TIME TO TIME
        timeSinceLastAction += Time.deltaTime;
        animator.SetFloat("smallActionIndex", -1f);

        if (timeSinceLastAction >= targetTimeUntilAction)
        {
            //Reset timer
            timeSinceLastAction = 0f;
            //Set new target time
            targetTimeUntilAction = Random.Range(minTimeBetweenSmallActions, maxTimeBetweenSmallActions);

            float smallActionSelector = Random.Range(0.0f, largestSmallActionValue);

            try
            {
                animator.SetFloat("smallActionIndex", blendValue);
            }
            catch
            {

            }

        }


    }

    public void playDeathAnimation()
    {
        animator.SetBool("IsDead", true);
    }
}
