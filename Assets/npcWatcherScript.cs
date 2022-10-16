using UnityEngine;
using System;
using System.Collections;
//Retrieved from here: https://docs.unity3d.com/Manual/InverseKinematics.html 
//Modified and programmed by Alexander Larsson Vahlberg


[RequireComponent(typeof(Animator))]

public class npcWatcherScript : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;


    //What to look at, such as an enemy or a point of interest.
    public Transform objTarget = null;

    public bool isWoman = true;

    private float baseLookWeight = 1f;
    private float bodyLookWeight = 0.5f;
    private float headLookWeight = 0.9f;


    [Range(0f, 1f)]
    public float baseLookWeightMAX = 1f;
    [Range(0f, 1f)]
    public float bodyLookWeightMAX = 0.5f;
    [Range(0f, 1f)]
    public float headLookWeightMAX = 0.9f;
    [Range(0f, 1f)]
    public float globalLookIKClamp = 0.5f;


    public Transform upperChest;

    public float headSnapSpeed = 2.5f;
    public float bodySnapSpeed = 1.5f;
    public float lookAtIKMaxDist = 35f;

    [Range(0f, 180f)]
    public float lookAtIKMaxBodyAngle = 70f;
    [Range(0f, 180f)]
    public float lookAtIKMaxHeadAngle = 70f;

    private AudioSource audioSource;
    public AudioClip maleCheer;
    public AudioClip femaleCheer;


    void Start()
    {
        animator = GetComponent<Animator>();
        objTarget = GameObject.FindWithTag("Player").transform;

        if (isWoman)
        {

        }
    }


    void Update()
    {
        LimitIK();
    }

    void LimitIK()
    {
        LimitBodyIK();
        LimitHeadIK();

    }
    void LimitBodyIK()
    {
        if (upperChest != null && objTarget != null)
        {
            //print("Angle: " + Vector3.Angle(transform.forward, objTarget.position - transform.position));
            float angleBetween = Vector3.Angle(transform.forward, objTarget.position - transform.position);
            float distBetween = Vector3.Distance(transform.forward, objTarget.position);
            //print("Angle: " + angleBetween);
            if (angleBetween <= lookAtIKMaxBodyAngle && distBetween < lookAtIKMaxDist)
            {
                baseLookWeight = Mathf.Lerp(baseLookWeight, baseLookWeightMAX, Time.deltaTime * bodySnapSpeed);
                bodyLookWeight = Mathf.Lerp(bodyLookWeight, bodyLookWeightMAX, Time.deltaTime * bodySnapSpeed);

            }
            else
            {
                headLookWeight = Mathf.Lerp(headLookWeight, 0, Time.deltaTime * bodySnapSpeed);
                bodyLookWeight = Mathf.Lerp(bodyLookWeight, 0, Time.deltaTime * bodySnapSpeed);
            }
        }


    }

    void LimitHeadIK()
    {

        if (upperChest != null && objTarget != null)
        {
            //print("Angle: " + Vector3.Angle(upperChest.forward, objTarget.position - upperChest.position));
            float angleBetween = Vector3.Angle(upperChest.forward, objTarget.position - upperChest.position);
            float distBetween = Vector3.Distance(upperChest.forward, objTarget.position);
            if (angleBetween <= lookAtIKMaxHeadAngle && distBetween < lookAtIKMaxDist)
            {
                headLookWeight = Mathf.Lerp(headLookWeight, headLookWeightMAX, Time.deltaTime * headSnapSpeed);
            }
            else
            {
                headLookWeight = Mathf.Lerp(headLookWeight, 0, Time.deltaTime * headSnapSpeed);
            }
        }

    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {

        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal.
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (objTarget != null)
                {
                    //print("IK");

                    //animator.SetLookAtWeight(headLookWeight);
                    animator.SetLookAtWeight(baseLookWeight, bodyLookWeight, headLookWeight, 0f, globalLookIKClamp);

                    //For player:
                    /* Ray lookAtRay = new Ray(transform.position, Camera.main.transform.forward);
                    animator.SetLookAtPosition(lookAtRay.GetPoint(25)); */

                    animator.SetLookAtPosition(objTarget.position);
                }



            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                /* animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); */
                animator.SetLookAtWeight(0);
            }
        }
    }
}

