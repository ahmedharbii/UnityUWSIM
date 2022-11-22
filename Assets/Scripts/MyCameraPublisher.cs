using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using System.Collections;
using Unity.Robotics.Core; //for time stamp
using System; // for the exception


// (15/11/2022 - Telerobotics subject)

[RequireComponent(typeof(ROSClockPublisher))] //This if I want to get a timestamp from a clock node that previouly made, Although it's more accurate, but I will skip it for now
 
public class MyCameraPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string ImageTopic = "/camera_rov/image";
    // public string CompressedImageTopic = "/camera_rov/CompressedImage";
    public string camInfoTopic = "/camera_rov/camera_info";
  
    public Camera _camera;
 
    public bool compressed = false;
 
    public float pubMsgFrequency = 30f;
 
    private float timeElapsed;
    private RenderTexture renderTexture;
    private RenderTexture lastTexture;
 
    private Texture2D mainCameraTexture;
    private Rect frame;
 
 
    private int frame_width;
    private int frame_height;
    private const int isBigEndian = 0;
    private uint image_step = 4;
    TimeMsg lastTime;
 
 
    private ImageMsg img_msg;
    private CameraInfoMsg infoCamera;
 
    private HeaderMsg header;
 
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
 
        if(ros)
        {
            ros.RegisterPublisher<ImageMsg>(ImageTopic);
            // ros.RegisterPublisher<CompressedImageMsg>(CompressedImageTopic);
            this._camera = GetComponent<Camera>();
            ros.RegisterPublisher<CameraInfoMsg>(camInfoTopic);
        }
        else
        {
            Debug.Log("No ros connection found.");
        }
 
 
        if (!_camera)
        {
            _camera = Camera.current;
        }
 
        if (_camera)
        {
            renderTexture = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 0, UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm);
            renderTexture.Create();
 
            frame_width = renderTexture.width;
            frame_height = renderTexture.height;
 
            frame = new Rect(0, 0, frame_width, frame_height);
 
            mainCameraTexture = new Texture2D(frame_width, frame_height, TextureFormat.RGBA32, false);
            // mainCameraTexture = new Texture2D();
            // mainCameraTexture.width = frame_width;
            // mainCameraTexture.height = frame_height;
 
            header = new HeaderMsg();
 
            img_msg = new ImageMsg();
 
            img_msg.width = (uint) frame_width;
            img_msg.height = (uint) frame_height;
            img_msg.step = image_step * (uint) frame_width;
            img_msg.encoding = "rgba8";
 
            // infoCamera = CameraInfoGenerator.ConstructCameraInfoMessage(_camera, header);
 
        }
        else
        {
            Debug.Log("No camera found.");
        }
    }
 
    private void Update()
    {
        if (Camera.main)
        {
            timeElapsed += Time.deltaTime;
 
            if (timeElapsed > (1 / pubMsgFrequency))
            {
                var timestamp = new TimeStamp(Clock.time); //using Unity.Robotics.Core; 
                header.stamp = timestamp; 
                img_msg.header = header;
                img_msg.data = get_frame_raw();
           
                ros.Publish(ImageTopic, img_msg);
                // ros.Publish(CompressedImageTopic, img_msg);
                // ros.Publish(camInfoTopic, infoCamera);
 
                timeElapsed = 0;
            }
        }
        else
        {
            Debug.Log("No camera found.");
        }
 
    }
 
    private byte[] get_frame_raw()
    {      
        _camera.targetTexture = renderTexture;
        lastTexture = RenderTexture.active;
 
        RenderTexture.active = renderTexture;
        _camera.Render();
 
        mainCameraTexture.ReadPixels(frame, 0, 0);
        mainCameraTexture.Apply(); //GPU 
        _camera.targetTexture = lastTexture;
 
        _camera.targetTexture = null;

        return mainCameraTexture.GetRawTextureData();
    }
}
 