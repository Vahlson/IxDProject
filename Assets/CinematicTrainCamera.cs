using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicTrainCamera : MonoBehaviour
{

    public GameObject cinematicCamera;
    public AudioSource trainNoise;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (other is BoxCollider)
            {
                //player.avoidObstacle(this);
                //print("Avoiding obstacale");
                if (cinematicCamera != null && cinematicCamera.TryGetComponent(out CinemachineVirtualCamera virtualCam))
                {
                    virtualCam.Priority = 11;
                    print("Priority is now 11");
                    //trainNoise.Play();
                }
            }

            /*  if (other is CapsuleCollider && player.damage && takeDamage)
             {
                 takeDamage = false;
                 player.takeDamage(this.blockadeType);
                 //print("Hit obstacle");
             }
  */


        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (other is BoxCollider)
            {
                if (cinematicCamera != null && cinematicCamera.TryGetComponent(out CinemachineVirtualCamera virtualCam))
                {
                    virtualCam.Priority = 1;
                    print("Priority is now 1");
                }
            }


        }
    }
}
