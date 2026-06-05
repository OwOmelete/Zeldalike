using UnityEngine;
using UnityEngine.EventSystems;

public class setupbutton : MonoBehaviour
{
public NavigationMenuStart navigationMenuStart;
 
    void OnEnable()
    {
       EventSystem.current.SetSelectedGameObject(navigationMenuStart.boutons[0]); 
    }
}
