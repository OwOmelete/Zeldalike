using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class camReset : MonoBehaviour
{
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
        transform.SetParent(InvoManager.Instance.transform);
        transform.localPosition = Vector3.zero;
    }
}
