using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Parametre : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider GVSlider;
    public Slider audioSourceSlider;
    //public LightingSettings lightingSettings;

    public void UpdateLighning()
    {
        RenderSettings.ambientIntensity = GVSlider.value*1.5f;
        Debug.Log("UpdateLight");
    }
    public void UpdateAudio()
    {
       audioSource.volume = audioSourceSlider.value;
       Debug.Log("UpdateAudio");
    }
    void OnEnable()
    {
        StartCoroutine(SetCurrentButton());
        
    }
    IEnumerator SetCurrentButton()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        EventSystem.current.SetSelectedGameObject(audioSourceSlider.gameObject);
    }

}
