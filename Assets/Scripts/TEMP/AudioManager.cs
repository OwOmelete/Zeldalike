using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public List<AudioClip> audioClips = new List<AudioClip>();
   public AudioSource audioSource;
   int musiqueActuelle;
    void Start()
    {
        musiqueActuelle=0;
        audioSource.clip = audioClips[musiqueActuelle];
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CollisionDetectionMode2D");
        if (other.CompareTag("Musique"))
        {
            Debug.Log("avec musique detecte");
           musiqueActuelle++; 
           audioSource.clip = audioClips[musiqueActuelle];
           audioSource.Play();
           other.enabled=false;
            if (musiqueActuelle == 2)
        {
         StartCoroutine(DebutMusique());
         }
        }
      
    }
    IEnumerator DebutMusique()
{
    yield return new WaitForSeconds(audioClips[musiqueActuelle].length);

    musiqueActuelle++;

    audioSource.clip = audioClips[musiqueActuelle];
    audioSource.Play();
}

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