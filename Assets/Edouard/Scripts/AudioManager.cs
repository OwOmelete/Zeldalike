using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   

    private void OnEnable()
    {
        GlobalEvents.OnButtonPressed += PlayButtonSound;
        GlobalEvents.OnEnemyAttack += PlayEnemyAttackSound;
        GlobalEvents.OnEnemyMove += PlayEnemyMoveSound;
        GlobalEvents.OnPauseSelecting += PlayButtonSound;
    }

    private void OnDisable()
    {
        GlobalEvents.OnButtonPressed -= PlayButtonSound;
    }

    public void PlayButtonSound()
    {
        Debug.Log("PlayingButtonSound");
    }

    public void PlayEnemyAttackSound()
    {
        Debug.Log("PlayEnemyAttackSound");
    }

    public void PlayEnemyMoveSound()
    {
        Debug.Log("PlayEnemyMoveSound");
    }
    
    public void PlayMoodSound(/*int mood*/)
    {
        int mood = 0; //temporary
        if (mood == 0)
        {
            Debug.Log("PlayMoodSoundHappy");
        }
        else if (mood == 1)
        {
            Debug.Log("PlayMoodSoundSuperHappy");
        }
        else if (mood == 2)
        {
            Debug.Log("PlayMoodSoundNormal");
        }
        else if (mood == 3)
        {
            Debug.Log("PlayMoodSoundSad");
        }
    }
}