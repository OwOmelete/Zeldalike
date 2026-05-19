using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;
    #region Calls

    private void OnEnable() {SoundEvents.OnButtonPress += PlayButtonPressSound;}
    private void OnDisable() {SoundEvents.OnButtonPress -= PlayButtonPressSound;}

    #endregion

    #region Methods

    public void PlayButtonPressSound()
    {
        Debug.Log("Playing button press");
        //audioSource.PlayOneShot(sounds[X]);
    }

    #endregion
}
