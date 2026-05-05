using UnityEngine;

public class ScreenEffectController : MonoBehaviour
{
    [SerializeField] private Material fullscreenMaterial;

    public void EnableEffect()
    {
        fullscreenMaterial.SetFloat("_Opacity", 1f);
    }

    public void DisableEffect()
    {
        fullscreenMaterial.SetFloat("_Opacity", 0f);
    }
}