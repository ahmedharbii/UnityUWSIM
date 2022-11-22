using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueRovVelocityControl : MonoBehaviour
{
    public float lvx = 0.0f; //Front axis
    public float lvy = 0.0f; //Right axis
    public float lvz = 0.0f; //Down Axis
    public float avz = 0.0f; //Angular Axis
    public bool movementActive = false;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    private void moveVelocityRigidbody() {
        Vector3 movement = new Vector3(-lvx * Time.deltaTime, lvz * Time.deltaTime, lvz * Time.deltaTime);
        transform.Translate(movement);
        transform.Rotate(0, avz * Time.deltaTime, 0);
    }


    void update()
    {
        if(movementActive) {
            moveVelocityRigidbody();
        }
    }

    public void moveVelocity(RosMessageTypes.Geometry.TwistMsg velocityMessage){
        this.lvx = (float)velocityMessage.linear.x;
        this.lvy = (float)velocityMessage.linear.y;
        this.lvz = (float)velocityMessage.linear.z;
        this.avz = (float)velocityMessage.angular.z;
        this.movementActive = true;
    }

    // // Update is called once per frame
    void FixedUpdate()
    {
        if(movementActive) {
            moveVelocityRigidbody();
        }
    }

    
    //         private void KeyBoardUpdate()
    //     {
    //         float moveDirection = Input.GetAxis("Vertical");
    //         float inputSpeed;
    //         float inputRotationSpeed;
    //         if (moveDirection > 0)
    //         {
    //             inputSpeed = maxLinearSpeed;
    //         }
    //         else if (moveDirection < 0)
    //         {
    //             inputSpeed = maxLinearSpeed * -1;
    //         }
    //         else
    //         {
    //             inputSpeed = 0;
    //         }

    //         float turnDirction = Input.GetAxis("Horizontal");
    //         if (turnDirction > 0)
    //         {
    //             inputRotationSpeed = maxRotationalSpeed;
    //         }
    //         else if (turnDirction < 0)
    //         {
    //             inputRotationSpeed = maxRotationalSpeed * -1;
    //         }
    //         else
    //         {
    //             inputRotationSpeed = 0;
    //         }
    //         RobotInput(inputSpeed, inputRotationSpeed);
    //     }
}   