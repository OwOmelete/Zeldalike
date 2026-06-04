using UnityEngine;

public class isOnPc : MonoBehaviour
{
     public uiManager uiManager;
    void OnEnable()
    {
        uiManager.isInPC=true;
    }
    void OnDisable()
    {
       uiManager.isInPC=false; 
    }
}
