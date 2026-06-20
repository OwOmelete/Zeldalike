using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematiqueManager : MonoBehaviour
{
    public static CinematiqueManager INSTANCE;

    [SerializeField] private VideoClip[] videoClips;
    [SerializeField] private VideoPlayer player;
    [SerializeField] private InputAction input;
    private float animStart;
    public GameObject cinematique1;
    public GameObject cinematique2;
    public int ActualAnim;
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
        player.targetCamera = Camera.main;
    }
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

    public void playAnim(int i)
    {
        //Time.timeScale = 0;
        player.clip = videoClips[i];
        player.targetCameraAlpha = 1;
        player.Play();
        
        
        animStart = Time.deltaTime;
        ActualAnim = i;
        StartCoroutine(StopCoroutine());
    }

    public void stopAnim()
    {
        Time.timeScale = 1;
        player.Stop();
        player.targetCameraAlpha = 0;
        
    }

    IEnumerator StopCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        stopAnim();
        if (ActualAnim == 3)
        {
            cinematique1.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            cinematique1.SetActive(false);
            cinematique2.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            cinematique2.SetActive(false);
        }
    }
}
