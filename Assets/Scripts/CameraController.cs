using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
 private WebCamTexture activeCamera;
 private WebCamDevice[] devices;
 private int currentCameraIndex = 0;
 public RawImage rawImage;

    void Awake()
    {
        devices = WebCamTexture.devices;
        WebCamTexture camToUse = new WebCamTexture(devices[currentCameraIndex].name);
        InitCamera(camToUse);
    }

    public void InitCamera(WebCamTexture camToUse)
    {
        if (activeCamera != camToUse)
        {
            if (activeCamera != null)
            {
                activeCamera.Stop();
            }
            activeCamera = camToUse;
            rawImage.texture = activeCamera;
            activeCamera.filterMode = FilterMode.Trilinear;
            activeCamera.Play();
        }
    }

    public void SwitchCamera()
    {
        currentCameraIndex = (currentCameraIndex + 1) % devices.Length;
        WebCamTexture camToUse = new WebCamTexture(devices[currentCameraIndex].name);
        InitCamera(camToUse);
    }

    public void Update()
    {
        if ( activeCamera.width < 100 )
        {
            Debug.Log("Still waiting another frame for correct info...");
            return;
        }
  
        int cwNeeded = activeCamera.videoRotationAngle;
        int ccwNeeded = -cwNeeded;
  
        if ( activeCamera.videoVerticallyMirrored ) ccwNeeded += 180;
  
        rawImage.rectTransform.localEulerAngles = new Vector3(0f,0f,ccwNeeded);
  
        float videoRatio = (float)activeCamera.width/(float)activeCamera.height;
  
        AspectRatioFitter rawImageARF = rawImage.GetComponent<AspectRatioFitter>();
        rawImageARF.aspectRatio = videoRatio;
  
        if ( activeCamera.videoVerticallyMirrored )
            rawImage.uvRect = new Rect(1,0,-1,1);
        else
            rawImage.uvRect = new Rect(0,0,1,1); 
    }
}

