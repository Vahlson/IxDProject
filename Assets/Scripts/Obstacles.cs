using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{


    public enum BlockadeType
    {
        High,
        Low,
        Full,
        None
    }

    public BlockadeType blockadeType = BlockadeType.None;
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
                player.avoidObstacle(this);
                //print("Avoiding obstacale");
            }

            if (other is CapsuleCollider)
            {
                player.takeDamage(this.blockadeType);
                //print("Hit obstacle");
            }

        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            if (other is CapsuleCollider)
            {
                player.keepRunning();
                print("Hit obstacle");
            }
        }
    }

}
