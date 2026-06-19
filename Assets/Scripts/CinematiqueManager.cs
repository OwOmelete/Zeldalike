using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class CinematiqueManager : MonoBehaviour
{
    public static CinematiqueManager INSTANCE;

    [SerializeField] private VideoClip[] videoClips;
    [SerializeField] private VideoPlayer player;
    [SerializeField] private InputAction input;
    private float animStart;


    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
        }
    }

    private void Update()
    {
        if (Gamepad.current.buttonSouth.isPressed && Time.unscaledTime - animStart >= 2)
        {
            stopAnim();
        }
    }

    public void playAnim(int i)
    {
        Time.timeScale = 0;
        player.clip = videoClips[i];
        player.targetCameraAlpha = 1;
        animStart = Time.unscaledTime;
    }

    public void stopAnim()
    {
        Time.timeScale = 1;
        player.targetCameraAlpha = 0;
    }
}
