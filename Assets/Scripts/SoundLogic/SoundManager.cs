using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;
    
    #region ButtonPressed

    private void OnEnable() {SoundEvents.OnButtonPress += PlayButtonPressSound;}
    private void OnDisable() {SoundEvents.OnButtonPress -= PlayButtonPressSound;}

    public void PlayButtonPressSound()
    {
        Debug.Log("Playing button press Sound");
        audioSource.PlayOneShot(sounds[0]);
    }

    #endregion

    #region Other
    
    

    #endregion
}
