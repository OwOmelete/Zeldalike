using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public void OpenDoor()
    {
        _animator.SetTrigger("Open");
    }
    
    
    public void Close()
    {
        _animator.SetTrigger("Close");
    }
}
