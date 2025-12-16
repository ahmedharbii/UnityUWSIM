using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.Core;
using System;

// [RequireComponent(typeof(ROSClockSubscriber))]
public class BlueRovCameraRosPublisher : MonoBehaviour {
  ROSConnection ros;
  public string topicName = "blue_rov1/CompressedImage";
  public CameraCapturer
      cameraCapturer; // i think this will be like Game Object in Unity Tutorial
  public float publishMessageFrequency = 0.5f; // 0.5f
  private float timeElapsed;
  private byte[] image;
  private CameraInfoMsg infoCamera;
  private CompressedImageMsg image_msg;

  private HeaderMsg header;
  private const int isBigEndian = 0;

  // private ROSClockSubscriber clock;
  // Start is called before the first frame update
  void Start() {
    // Creating ros connection
    ros = ROSConnection.GetOrCreateInstance();
    // Getting components of the camera capture class
    this.cameraCapturer = GetComponent<CameraCapturer>();
    // Now we register our publisher topic name
    ros.RegisterPublisher<CompressedImageMsg>(topicName);
    // Debugging to check if our publisher is registered sucessfully
    Debug.Log("Publish Successfully");

    header = new HeaderMsg();

    image_msg = new CompressedImageMsg();

    // Not working with ROS2:
    // image_msg.header.stamp.sec =( int)(System.DateTime.UtcNow - new
    // DateTime(1970,1,1)).TotalSeconds; image_msg.header.stamp.nanosec =
    // (uint)(System.DateTime.UtcNow - new
    // DateTime(1970,1,1)).Milliseconds*1000*1000;
  }

  private void Update() {
    timeElapsed += Time.deltaTime;

    if (timeElapsed > publishMessageFrequency) {
      // Finally send the message to server_endpoint.py running in ROS
      // header.stamp = clock._time();
      // header = new HeaderMsg();
      // image_msg = new ImageMsg();

      // Not working with ROS2 ///////////////////////////////////////////
      // var timestamp = new TimeStamp(Clock.time);
      // header.stamp = timestamp;

      image_msg.header = header;
      // infoCamera.header = header;

      // image = new ImageMsg();
      image = cameraCapturer.getCapturedJpegImage();
      image_msg.data = image;

      // image = this.cameraCapturer.getCapturedJpegImage();
      ros.Publish(topicName, image_msg);
      Debug.Log("Image sent");

      timeElapsed = 0;
    }
  }
}
