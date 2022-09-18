using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    float velocity = 0.0f;
    public float acceleration = 1.0f;
    public float deceleration = 0.5f;
    private Rigidbody _rb;
    int velocityHash;
    CharacterController characterController;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");

    }

    // Update is called once per frame
    void Update()
    {

        characterController.Move(this.transform.forward * velocity * Time.deltaTime);
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");

        if (leftPressed)
        {
            this.transform.eulerAngles += new Vector3(0, -90, 0);

        }
        if (rightPressed)
        {
            this.transform.eulerAngles += new Vector3(0, 90, 0);
        }
        if (forwardPressed)
        {
            velocity += Time.deltaTime * acceleration;
        }
        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }
        if (!forwardPressed && velocity < 0.0f)
        {
            velocity = 0.0f;
        }
        _animator.SetFloat(velocityHash, velocity);


    }
    void OnTriggerEnter(Collider other)
    {
        print("collision enter, moving " + other.gameObject.tag);
        switch (other.gameObject.tag)
        {
            case "Left":
                this.transform.eulerAngles += new Vector3(0, -90, 0);
                break;
            case "Right":
                this.transform.eulerAngles += new Vector3(0, 90, 0);
                break;

        }

    }

}
