using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class navigationMenu : MonoBehaviour
{
    public GameObject go;
    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(go);
    }
}
