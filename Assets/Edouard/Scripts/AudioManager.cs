using UnityEngine;

public class AudioManager : MonoBehaviour
{
   

    private void OnEnable()
    {
        GlobalEvents.OnButtonPressed += PlayButtonSound;
    }

    public void PlayButtonSound()
    {
        Debug.Log("PlayingButtonSound");
    }
}