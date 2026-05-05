using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCam;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCam = Camera.main;
    }
    
    private void Start()
    {
        mainCam = Camera.main;
    }
    
    

    private void LateUpdate()
    {
        Vector3 camPos = mainCam.transform.position;
        camPos.y = transform.position.y;
        transform.LookAt(camPos);
        transform.Rotate(0, 180, 0);
    }
}
