using UnityEngine;

public class isOnPc : MonoBehaviour
{
    public uiManager uiManager;
    float moveSpeed;
    void OnEnable()
    {
        uiManager.isInPC=true;
        moveSpeed = TopDownPlayerController.Instance.moveSpeed;
        TopDownPlayerController.Instance.moveSpeed=0;
    }
    void OnDisable()
    {
       uiManager.isInPC=false; 
       TopDownPlayerController.Instance.moveSpeed=12f;
    }
}
