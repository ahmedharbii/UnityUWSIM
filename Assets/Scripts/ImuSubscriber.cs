using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Geometry;
// Code is inspired from here
// https://github.com/Field-Robotics-Japan/UnitySensors/blob/09b451f4b2544b4f999179820db290a12a756591/Scripts/Runtime/IMU/IMU.cs

[RequireComponent(typeof(Rigidbody))]
public class ImuSubscriber : MonoBehaviour
{

    ROSConnection ros;
    // public string imuTopic = "/mavros/imu/data";
    public string imuTopic = "/br5/mavros/imu/data";
    

    public float speed = 10.0f;
    private Rigidbody _rb;
    private Transform _transform;
    private Vector3 movement;
    private Vector3 _lastVelocity = Vector3.zero;

    private QuaternionMsg imuQuaternion;
    private QuaternionMsg _geometryQuaternion;
    private Vector3 _angularVelocity;
    private Vector3 _linearAcceleration;
    //Quaternions
    private double q1;
    private double q2;
    private double q3;
    private double q4;
    // Euler Angles
    private double roll;
    private double pitch;
    private double yaw;
    //https://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html
    float rotationSpeed = 0;
    Vector3 currentEulerAngles;
    Vector3 imuLinearVelocity;
    Quaternion currentRotation;
    float x;
    float y;
    float z;

    // private Noise.Gaussian gaussianNoise;
    // private Noise.Bias biasNoise;


    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>(); //Getting components of our BlueROV
        _transform = this.GetComponent<Transform>(); // Getting the components of transform
        ROSConnection.GetOrCreateInstance().Subscribe<ImuMsg>(imuTopic, ImuCallback);
    }


    public void ImuCallback(RosMessageTypes.Sensor.ImuMsg imuChange)
    {
        // Debug.Log("" + imuChange);
        // Debug.Log("" + imuChange.orientation); //Vector3Msg
        // this.imuLinearVelocity = imuChange.linear_velocity;
        this.imuQuaternion = imuChange.orientation; //getting orientations from 
        // this._imuOrientations = imuChange.orientation;
        this._transform = this.GetComponent<Transform>();
        this._rb = this.GetComponent<Rigidbody>();
        // currentEulerAngles = this._rb.transform.rotation.eulerAngles;
        // this._geometryQuaternion = new Vector4();
        this._geometryQuaternion = imuChange.orientation;
        this._angularVelocity = new Vector3();
        this._linearAcceleration = new Vector3();
    }


    private void FixedUpdate()
    {
        // instead of x, y, z -> give it euler angles from quaternions
        // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles - c++ code
        q1 = this.imuQuaternion.x;
        q2 = this.imuQuaternion.y;
        q3 = this.imuQuaternion.z;
        q4 = this.imuQuaternion.w;

        x = this.imuLinearVelocity.x;
        y = this.imuLinearVelocity.y;
        z = this.imuLinearVelocity.z;

        // roll (x-axis rotation)
        double sinr_cosp = 2 * (q4 * q1 + q2 * q3);
        double cosr_cosp = 1 - 2 * (q1 * q1 + q2 * q2);
        roll = Math.Atan2(sinr_cosp, cosr_cosp);

        // pitch (y-axis rotation)
        double sinp = 2 * (q4 * q2 - q3 * q1);
        if (Math.Abs(sinp) >= 1)
            {
                // Math.CopySign not working
                pitch = Math.Abs(Math.PI) * Math.Sign(sinp); //Math.CopySign(Math.PI / 2, sinp); // use 90 degrees if out of range
            }
        else
            pitch = Math.Sin(sinp);

        // yaw (z-axis rotation)
        double siny_cosp = 2 * (q4 * q3 + q1 * q2);
        double cosy_cosp = 1 - 2 * (q2 * q2 + q3 * q3);
        yaw = Math.Atan2(siny_cosp, cosy_cosp);

        // currentEulerAngles += new Vector3((float)pitch, (float)yaw, (float)roll) * Time.deltaTime * rotationSpeed;

        currentEulerAngles = new Vector3((float)pitch, (float)yaw, (float)roll);// * Time.deltaTime * rotationSpeed;
        // t // you will get this from velocity over time
        currentRotation.eulerAngles = currentEulerAngles;
        this._rb.transform.Rotate(currentEulerAngles); // https://github.com/IntelRealSense/librealsense/issues/10858


        // should be from linear acceleration
        this._rb.transform.Translate(currentChangePosition);
    

        // you will get this from velocity over time

        // this._rb.transform.position = currentChangePosition;
        Debug.Log("here");

        // Vector3 localLinearVelocity = this._transform.InverseTransformDirection(this._rb.velocity);
        // Vector3 acceleration = (localLinearVelocity - this._lastVelocity) / Time.deltaTime;
        // this._lastVelocity = localLinearVelocity;
        // // Add Gravity Element
        // acceleration += this._transform.InverseTransformDirection(Physics.gravity);
        // // Update //

        // // Raw
        // this._geometryQuaternion = new QuaternionMsg(this._transform.rotation.x, this._transform.rotation.y,
        //                                         this._transform.rotation.z, this._transform.rotation.w);
        // // this._rb.transform.rotation.x = _geometryQuaternion;
        // // this._geometryQuaternion = new Vector4(q1, q2, q3, q4);
        // this._angularVelocity = -1 * this.transform.InverseTransformVector(this.GetComponent<Rigidbody>().angularVelocity);
        // this._linearAcceleration = acceleration;
    }
}