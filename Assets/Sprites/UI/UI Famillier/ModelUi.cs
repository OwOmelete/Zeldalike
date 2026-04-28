using UnityEngine;
using UnityEngine.UI;

public class ModelUI : MonoBehaviour
{
    public Camera modelCamera;
    public RawImage display;
    public RenderTexture renderTexture;
    public GameObject CameraHolder;
    public Vector3 VitesseRotation = new Vector3 (0,10,0);

    void Start()
    {
        modelCamera.targetTexture = renderTexture;
    }
    
}