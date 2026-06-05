using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private bool startOpen;


    private void Start()
    {
        if(startOpen) OpenDoor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) OpenDoor();
        if (Input.GetKeyDown(KeyCode.H)) Close();;
    }

    public void OpenDoor()
    {
        _animator.SetTrigger("Open");
    }
    
    
    public void Close()
    {
        _animator.SetTrigger("Close");
    }
}
