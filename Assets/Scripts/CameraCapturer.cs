using System;
using UnityEngine;

// Modified
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Sensor; // ImageMsg
using RosMessageTypes.Std;

// Modified

public class CameraCapturer : MonoBehaviour {
  public bool IsCaptureEnable = false;
  Camera _camera;
  public int resWidth;
  public int resHeight;
  public byte[] jpg;

  // Modified
  // public string topicName = "camera_rov";
  // ROSConnection ros;
  // private ImageMsg image_msg;

  // Modified

  void Start() {
    this._camera = GetComponent<Camera>();

    // Modified
    // ros = ROSConnection.GetOrCreateInstance();
    // ros.RegisterPublisher<ImageMsg>(topicName);
    // Debug.Log("Publish Successfully");

    // Modified
  }

  private void FixedUpdate() {
    if (this.IsCaptureEnable) {
      this.jpg = getJPGFromCurrentCamera();
      // put the publisher here
      // image_msg.data = this.jpg;
      // ros.Publish(topicName, image_msg);
      // Modified
      this.IsCaptureEnable = false;
    }
  }

  public byte[] getCapturedJpegImage() {
    this.IsCaptureEnable = true;
    while (this.IsCaptureEnable) {

      if (this.jpg != null) {
        return this.jpg;
      }
    }
    return null;
  }

  private byte[] getJPGFromCurrentCamera() {
    try {
      RenderTexture rt = new RenderTexture(resWidth, resHeight,
                                           24); // 24 is the number of pixels
      _camera.targetTexture = rt;
      Texture2D screenShot =
          new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
      _camera.Render();
      RenderTexture.active = rt;
      screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
      screenShot.Apply(); ///////
      _camera.targetTexture = null;
      RenderTexture.active = null;
      Destroy(rt); // for the garbage collector
      byte[] bytes = screenShot.EncodeToJPG();
      return bytes;
    } catch (Exception e) {
      Debug.Log("Error");
      Debug.Log(e);
      return null;
    }
  }
}