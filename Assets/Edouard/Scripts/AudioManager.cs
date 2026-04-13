using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   

    private void OnEnable()
    {
        GlobalEvents.OnButtonPressed += PlayButtonSound;
        GlobalEvents.OnEnemyAttack += EnemyAttackSound;
        GlobalEvents.OnEnemyMove += EnemyMoveSound;
    }

    private void OnDisable()
    {
        GlobalEvents.OnButtonPressed -= PlayButtonSound;
    }

    public void PlayButtonSound()
    {
        Debug.Log("PlayingButtonSound");
    }

    public void EnemyAttackSound()
    {
        Debug.Log("PlayEnemyAttackSound");
    }

    public void EnemyMoveSound()
    {
        Debug.Log("PlayEnemyMoveSound");
    }
}